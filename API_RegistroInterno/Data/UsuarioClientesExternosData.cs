using API_RegistroInterno.Context;
using API_RegistroInterno.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_RegistroInterno.Data
{
    public class UsuarioClientesExternosData
    {
        private readonly AppDbContext _context;
        private readonly OyDDbContext _oyDContext;

        public UsuarioClientesExternosData(AppDbContext context, OyDDbContext oyDContext)
        {
            _context = context;
            _oyDContext = oyDContext;
        }

        /// <summary>
        /// Obtiene todos los registros de tblUsuariosClientesExternos.
        /// </summary>
        public async Task<List<UsuarioClientesExternos>> GetListaAsync()
        {
            return await _context.tblUsuariosClientesExternos.ToListAsync();
        }

        /// <summary>
        /// Indica si ya existe un usuario en tblUsuariosClientesExternos con los datos indicados (incluida fecha de expedición).
        /// </summary>
        public async Task<bool> ExisteEnUsuariosExternosAsync(
            string tipoDoc,
            string numDoc,
            DateTime fechaExpedicion,
            string nombres,
            string apellido1,
            string apellido2,
            string celular,
            string correo)
        {
            var fechaInicio = fechaExpedicion.Date;
            var fechaFin = fechaExpedicion.Date.AddDays(1);

            return await _context.tblUsuariosClientesExternos
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
        }

        /// <summary>
        /// Indica si existe un cliente en tblClientes (DbOyD) con los datos indicados (incluida fecha de expedición).
        /// </summary>
        public async Task<bool> ExisteEnClientesAsync(
            string tipoDoc,
            string numDoc,
            DateTime fechaExpedicion,
            string nombres,
            string apellido1,
            string apellido2,
            string celular,
            string correo)
        {
            return await _oyDContext.tblClientes
                .AnyAsync(c =>
                    (c.TipoDocumento ?? "").Trim() == tipoDoc &&
                    (c.Documento ?? "").Trim() == numDoc &&
                    c.FechaExpedicion != null && c.FechaExpedicion.Value.Date == fechaExpedicion.Date &&
                    (c.Nombres ?? "").Trim() == nombres &&
                    (c.PrimerApellido ?? "").Trim() == apellido1 &&
                    (c.SegundoApellido ?? "").Trim() == apellido2 &&
                    (c.Celular ?? "").Trim() == celular &&
                    (c.Correo ?? "").Trim() == correo);
        }

        /// <summary>
        /// Registra un nuevo usuario en tblUsuariosClientesExternos a partir de los datos del request.
        /// </summary>
        public async Task RegistrarAsync(
            string tipoDoc,
            string numDoc,
            DateTime fechaExpedicion,
            string nombres,
            string apellido1,
            string apellido2,
            string celular,
            string correo)
        {
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
        }
    }
}
