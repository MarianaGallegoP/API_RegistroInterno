using API_RegistroInterno.Data;
using API_RegistroInterno.Models;
using API_RegistroInterno.Models.IdentityValidation;
using API_RegistroInterno.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace API_RegistroInterno.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioClientesExternosController : ControllerBase
    {
        private readonly UsuarioClientesExternosData _data;
        private readonly IIdentityValidationService _identityValidation;

        public UsuarioClientesExternosController(UsuarioClientesExternosData data, IIdentityValidationService identityValidation)
        {
            _data = data;
            _identityValidation = identityValidation;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult> Get()
        {
            var lista = await _data.GetListaAsync();
            return Ok(lista);
        }

        [HttpPost]
        [Route("/api/auth/register/init")]
        public async Task<ActionResult<RegistrarUsuarioClienteExternoResponse>> Registrar([FromBody] RegistrarUsuarioClienteExternoRequest request)
        {
            if (request == null)
                return BadRequest(new RegistrarUsuarioClienteExternoResponse { Success = false, Message = "Cuerpo de la petición inválido.", Code = "BAD_REQUEST" });

            if (!DateTime.TryParseExact(request.FechaExpedicion.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime fechaExpedicion))
                return BadRequest(new RegistrarUsuarioClienteExternoResponse { Success = false, Message = "Formato de fecha_expedicion inválido. Use dd/MM/yyyy.", Code = "FECHA_INVALIDA" });

            var tipoDoc = request.TipoDocumento?.Trim() ?? "";
            var numDoc = request.NumeroDocumento?.Trim() ?? "";
            var nombres = request.Nombres?.Trim() ?? "";
            var apellido1 = request.PrimerApellido?.Trim() ?? "";
            var apellido2 = request.SegundoApellido?.Trim() ?? "";
            var celular = request.Celular?.Trim() ?? "";
            var correo = request.Correo?.Trim() ?? "";

            // Validación: usuario NO debe estar en tblUsuariosClientesExternos
            var yaEnExternos = await _data.ExisteEnUsuariosExternosAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);
            if (yaEnExternos)
                return Conflict(new RegistrarUsuarioClienteExternoResponse
                {
                    Success = false,
                    Code = "USER_ALREADY_EXISTS",
                    Message = "El usuario ya está registrado."
                });

            // Validación: usuario SÍ debe estar en tblClientes
            var existeEnClientes = await _data.ExisteEnClientesAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);
            if (!existeEnClientes)
                return Conflict(new RegistrarUsuarioClienteExternoResponse
                {
                    Success = false,
                    Code = "USER_NOT_FOUND_IN_SYSTEM",
                    Message = "Usuario no encontrado en el sistema."
                });

            // Llamada a la plataforma interna de validación de identidad (backend to backend)
            var identityRequest = new IdentityValidationRequest
            {
                TipoDocumento = tipoDoc,
                NumeroDocumento = numDoc,
                FechaExpedicion = request.FechaExpedicion.Trim(),
                Nombres = nombres,
                PrimerApellido = apellido1,
                SegundoApellido = apellido2,
                Celular = celular,
                Correo = correo
            };

            var identityResponse = await _identityValidation.ValidateIdentityAsync(identityRequest);

            // Mapear códigos de fallo de la plataforma interna a códigos de la API de registro interno
            if (identityResponse == null || !IsValidationSuccess(identityResponse.Status))
            {
                var (code, message) = MapIdentityErrorToResponse(identityResponse?.Status);
                return BadRequest(new RegistrarUsuarioClienteExternoResponse
                {
                    Success = false,
                    Code = code,
                    Message = message
                });
            }

            // Respuesta positiva: OTP o QUESTIONS según lo que devolvió la plataforma
            var registrationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var validationSessionId = identityResponse.ValidationSessionId ?? "sess-internal-" + Guid.NewGuid().ToString("N")[..12];
            var verificationMethod = identityResponse.VerificationMethod ?? "OTP";

            RegistrarUsuarioClienteExternoResponse response;
            if (string.Equals(verificationMethod, "OTP", StringComparison.OrdinalIgnoreCase))
            {
                response = new RegistrarUsuarioClienteExternoResponse
                {
                    Success = true,
                    Code = "REGISTRATION_INITIATED",
                    Message = "Proceso de registro iniciado correctamente",
                    VerificationMethod = "OTP",
                    RegistrationToken = registrationToken,
                    ValidationSessionId = validationSessionId,
                    ExpiresIn = identityResponse.ExpiresIn ?? 60,
                    PhoneMasked = identityResponse.PhoneMasked ?? RegistrarUsuarioClienteExternoResponse.EnmascararCelular(celular)
                };
            }
            else
            {
                response = new RegistrarUsuarioClienteExternoResponse
                {
                    Success = true,
                    Code = "REGISTRATION_INITIATED",
                    Message = "Proceso de registro iniciado correctamente",
                    VerificationMethod = "QUESTIONS",
                    RegistrationToken = registrationToken,
                    ValidationSessionId = validationSessionId,
                    Questions = identityResponse.Questions
                };
            }

            // Registrar usuario solo después de respuesta positiva de la plataforma interna
            await _data.RegistrarAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);

            return Ok(response);
        }

        private static bool IsValidationSuccess(string status)
        {
            return string.Equals(status, "VALIDATION_SUCCESS_OTP", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(status, "VALIDATION_SUCCESS_QUESTIONS", StringComparison.OrdinalIgnoreCase);
        }

        private static (string Code, string Message) MapIdentityErrorToResponse(string? platformStatus)
        {
            return (platformStatus?.ToUpperInvariant()) switch
            {
                "VALIDATION_FAILED" => ("IDENTITY_VALIDATION_FAILED", "La validación de identidad no fue exitosa."),
                "DATA_MISMATCH" => ("IDENTITY_DATA_MISMATCH", "Los datos no coinciden con los registrados."),
                "SERVICE_ERROR" => ("INTERNAL_SERVER_ERROR", "Error en el servicio de validación. Intente más tarde."),
                _ => ("INTERNAL_SERVER_ERROR", "Error en el servicio de validación. Intente más tarde.")
            };
        }
    }
}
