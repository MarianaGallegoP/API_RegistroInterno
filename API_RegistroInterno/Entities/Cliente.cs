namespace API_RegistroInterno.Entities
{
    /// <summary>
    /// Entidad para validación contra tblClientes (DbOyD). Solo incluye los campos usados en el POST.
    /// Columnas: strTipoIdentificacion, strNroDocumento, strNombre, strTelefono1, strApellido1, strApellido2, dtmFechaExpedicionDoc, strEMail.
    /// </summary>
    public class Cliente
    {
        public string? TipoDocumento { get; set; }
        public string? Documento { get; set; }
        public string? Nombres { get; set; }
        public string? Celular { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public string? Correo { get; set; }
    }
}
