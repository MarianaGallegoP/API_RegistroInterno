using API_RegistroInterno.Data;
using API_RegistroInterno.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace API_RegistroInterno.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioClientesExternosController : ControllerBase
    {
        private readonly UsuarioClientesExternosData _data;

        public UsuarioClientesExternosController(UsuarioClientesExternosData data)
        {
            _data = data;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult> Get()
        {
            var lista = await _data.GetListaAsync();
            return Ok(lista);
        }

        [HttpPost]
        [Route("Registrar")]
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

            // Validación exitosa: elegir aleatoriamente OTP o Cuestionario
            var metodoVerificacion = Random.Shared.Next(2) == 0 ? "OTP" : "QUESTIONS";
            var registrationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var validationSessionId = "sess-internal-" + Guid.NewGuid().ToString("N")[..12];

            RegistrarUsuarioClienteExternoResponse response;

            if (metodoVerificacion == "OTP")
            {
                var phoneMasked = EnmascararCelular(celular);
                response = new RegistrarUsuarioClienteExternoResponse
                {
                    Success = true,
                    Code = "REGISTRATION_INITIATED",
                    Message = "Proceso de registro iniciado correctamente",
                    VerificationMethod = "OTP",
                    RegistrationToken = registrationToken,
                    ValidationSessionId = validationSessionId,
                    ExpiresIn = 60,
                    PhoneMasked = phoneMasked
                };
            }
            else
            {
                var questions = ObtenerPreguntasCuestionario();
                response = new RegistrarUsuarioClienteExternoResponse
                {
                    Success = true,
                    Code = "REGISTRATION_INITIATED",
                    Message = "Proceso de registro iniciado correctamente",
                    VerificationMethod = "QUESTIONS",
                    RegistrationToken = registrationToken,
                    ValidationSessionId = validationSessionId,
                    Questions = questions
                };
            }

            // Registrar usuario solo después de elegir el método y armar la respuesta
            await _data.RegistrarAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);

            return Ok(response);
        }

        /// <summary>
        /// Enmascara el celular mostrando solo los primeros 3 y últimos 3 dígitos (ej: 300****567).
        /// </summary>
        private static string EnmascararCelular(string celular)
        {
            if (string.IsNullOrEmpty(celular) || celular.Length < 7)
                return "**********";
            return celular[..3] + "****" + celular[^3..];
        }

        /// <summary>
        /// Devuelve las preguntas de verificación para el método QUESTIONS.
        /// </summary>
        private static List<VerificationQuestion> ObtenerPreguntasCuestionario()
        {
            return new List<VerificationQuestion>
            {
                new VerificationQuestion
                {
                    Id = "Q1",
                    Question = "¿En qué ciudad nació?",
                    Options = new List<VerificationQuestionOption>
                    {
                        new() { Id = "A", Text = "Bogotá" },
                        new() { Id = "B", Text = "Medellín" },
                        new() { Id = "C", Text = "Cali" },
                        new() { Id = "D", Text = "Barranquilla" }
                    }
                },
                new VerificationQuestion
                {
                    Id = "Q2",
                    Question = "¿Cuál fue su primer trabajo?",
                    Options = new List<VerificationQuestionOption>
                    {
                        new() { Id = "A", Text = "Empresa privada" },
                        new() { Id = "B", Text = "Sector público" },
                        new() { Id = "C", Text = "Independiente" },
                        new() { Id = "D", Text = "Otro" }
                    }
                }
            };
        }
    }
}
