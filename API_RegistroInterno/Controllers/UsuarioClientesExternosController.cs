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
                return BadRequest(new RegistrarUsuarioClienteExternoResponse { Exito = false, Mensaje = "Cuerpo de la petición inválido.", Codigo = "REQUEST_INVALIDO" });

            if (!DateTime.TryParseExact(request.FechaExpedicion.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime fechaExpedicion))
                return BadRequest(new RegistrarUsuarioClienteExternoResponse { Exito = false, Mensaje = "Formato de fecha_expedicion inválido. Use dd/MM/yyyy.", Codigo = "FECHA_INVALIDA" });

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
                    Exito = true,
                    Mensaje = "El usuario ya se encuentra registrado en tblUsuariosClientesExternos.",
                    Codigo = "YA_REGISTRADO"
                });

            var existeEnClientes = await _data.ExisteEnClientesAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);
            if (!existeEnClientes)
                return Ok(new RegistrarUsuarioClienteExternoResponse
                {
                    Exito = false,
                    Mensaje = "El usuario no se encuentra en tblClientes (DbOyD). No es posible registrarlo en tblUsuariosClientesExternos.",
                    Codigo = "NO_EN_TBL_CLIENTES"
                });

            await _data.RegistrarAsync(tipoDoc, numDoc, fechaExpedicion, nombres, apellido1, apellido2, celular, correo);

            return Ok(new RegistrarUsuarioClienteExternoResponse
            {
                Exito = true,
                Mensaje = "Usuario registrado correctamente en tblUsuariosClientesExternos.",
                Codigo = "REGISTRADO"
            });
        }
    }
}
