using Microsoft.AspNetCore.Mvc;
using API_RegistroInterno.Data;
using API_RegistroInterno.Models;

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

            var yaEnExternos = await _data.ExisteEnUsuariosExternosAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);
            if (yaEnExternos)
                return Ok(new RegistrarUsuarioClienteExternoResponse
                {
                    Success = true,
                    Code = "USER_ALREADY_EXISTS",
                    Message = "El usuario ya esta registrado",
                    VerificationMethod = "Metodo de verificacion",
                    RegistrationToken = "token de registro",
                    ValidationSessionId = "id de validacion de sesion",
                    ExpiresIn = 60,
                    PhoneMasked = request.Celular
                });

            var existeEnClientes = await _data.ExisteEnClientesAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);
            if (!existeEnClientes)
                return Ok(new RegistrarUsuarioClienteExternoResponse
                {
                    Success = true,
                    Code = "USER_NOT_FOUND_IN_SYSTEM",
                    Message = "Usuario no escontrado en el sistema",
                    VerificationMethod = "Metodo de verificacion",
                    RegistrationToken = "token de registro",
                    ValidationSessionId = "id de validacion de sesion",
                    ExpiresIn = 60,
                    PhoneMasked = request.Celular
                });

            await _data.RegistrarAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);

            return Ok(new RegistrarUsuarioClienteExternoResponse
            {
                Success = true,
                Code = "REGISTRATION_SUCCESS",
                Message = "El usuario ha sido registrado",
                VerificationMethod = "Metodo de verificacion",
                RegistrationToken = "token de registro",
                ValidationSessionId = "id de validacion de sesion",
                ExpiresIn = 60,
                PhoneMasked = request.Celular
            });
        }
    }
}
