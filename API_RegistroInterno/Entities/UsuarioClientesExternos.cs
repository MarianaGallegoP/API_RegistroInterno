namespace API_RegistroInterno.Entities
{
    public class UsuarioClientesExternos
    {
        public int id { get; set; }
        public string tipoDocumento { get; set; }
        public string documento { get; set; }
        public DateTime expedicion { get; set; }
        public string nombre1 { get; set; }
        public string nombre2 { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string celular { get; set; }
        /// <summary>Contraseña almacenada como varbinary (hash/bytes) en la base de datos.</summary>
        public byte[]? clave { get; set; }
        public int intentosFallidos { get; set; }
        public DateTime ultimoAcceso { get; set; }
        public int estado { get; set; }
        public DateTime creacion { get; set; }
        public required DateTime actualizacion { get; set; }
    }
}
