using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_RegistroInterno.Context;
using API_RegistroInterno.Entities;
using API_RegistroInterno.Models;

namespace API_RegistroInterno.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioClientesExternosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly OyDDbContext _oyDContext;

        public UsuarioClientesExternosController(AppDbContext context, OyDDbContext oyDContext)
        {
            _context = context;
            _oyDContext = oyDContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult> Get()
        {
            var listaUsuariosClientesExternos = await _context.tblUsuariosClientesExternos.ToListAsync();
            return Ok(listaUsuariosClientesExternos);
        }

        /// <summary>
        /// Valida si el usuario está en tblUsuariosClientesExternos (bdSofia). Si no está, valida en tblClientes (DbOyD)
        /// y, si está allí, lo registra en tblUsuariosClientesExternos.
        /// </summary>
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

            // 1. Primero: validar que NO esté en tblUsuariosClientesExternos (bdSofia). Con todos los datos del JSON, incluida fecha_expedicion.
            var fechaInicio = fechaExpedicion.Date;
            var fechaFin = fechaExpedicion.Date.AddDays(1);
            var yaEnExternos = await _context.tblUsuariosClientesExternos
                .AnyAsync(u =>
                    (u.tipoDocumento ?? "").Trim() == tipoDoc &&
                    (u.documento ?? "").Trim() == numDoc &&
                    u.expedicion >= fechaInicio && u.expedicion < fechaFin &&
                    ((u.nombre ?? "").Trim() == nombres ||
                     ((u.nombre1 ?? "").Trim() + " " + (u.nombre2 ?? "").Trim()).Trim() == nombres) &&
                    (u.apellido1 ?? "").Trim() == apellido1 &&
                    (u.apellido2 ?? "").Trim() == apellido2 &&
                    (u.celular ?? "").Trim() == celular &&
                    (u.email ?? "").Trim() == correo);

            if (yaEnExternos)
                return Ok(new RegistrarUsuarioClienteExternoResponse
                {
                    Exito = true,
                    Mensaje = "El usuario ya se encuentra registrado en tblUsuariosClientesExternos.",
                    Codigo = "YA_REGISTRADO"
                });

            // 2. Solo si NO está en tblUsuariosClientesExternos: validar que SÍ esté en tblClientes (DbOyD), incluyendo fecha_expedicion (dtmFechaExpedicionDoc).
            var existeEnClientes = await _oyDContext.tblClientes
                .AnyAsync(c =>
                    (c.TipoDocumento ?? "").Trim() == tipoDoc &&
                    (c.Documento ?? "").Trim() == numDoc &&
                    c.FechaExpedicion != null && c.FechaExpedicion.Value.Date == fechaExpedicion.Date &&
                    (c.Nombres ?? "").Trim() == nombres &&
                    (c.PrimerApellido ?? "").Trim() == apellido1 &&
                    (c.SegundoApellido ?? "").Trim() == apellido2 &&
                    (c.Celular ?? "").Trim() == celular &&
                    (c.Correo ?? "").Trim() == correo);

            if (!existeEnClientes)
                return Ok(new RegistrarUsuarioClienteExternoResponse
                {
                    Exito = false,
                    Mensaje = "El usuario no se encuentra en tblClientes (DbOyD). No es posible registrarlo en tblUsuariosClientesExternos.",
                    Codigo = "NO_EN_TBL_CLIENTES"
                });

            // 3. Registrar en tblUsuariosClientesExternos
            var partesNombres = nombres.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var nombre1 = partesNombres.Length > 0 ? partesNombres[0] : nombres;
            var nombre2 = partesNombres.Length > 1 ? string.Join(" ", partesNombres.Skip(1)) : "";

            var ahora = DateTime.UtcNow;
            var nuevoUsuario = new UsuarioClientesExternos
            {
                tipoDocumento = tipoDoc,
                documento = numDoc,
                expedicion = fechaExpedicion,
                nombre1 = nombre1,
                nombre2 = nombre2,
                apellido1 = apellido1,
                apellido2 = apellido2,
                nombre = nombres,
                email = correo,
                celular = celular,
                clave = Array.Empty<byte>(),
                intentosFallidos = 0,
                ultimoAcceso = ahora,
                estado = 1,
                creacion = ahora,
                actualizacion = ahora
            };

            _context.tblUsuariosClientesExternos.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new RegistrarUsuarioClienteExternoResponse
            {
                Exito = true,
                Mensaje = "Usuario registrado correctamente en tblUsuariosClientesExternos.",
                Codigo = "REGISTRADO"
            });
        }
    }
}
