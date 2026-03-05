namespace API_RegistroInterno.Entities
{
    public class LogAuditorias
    {
        public required int idLog { get; set; }
        public required string tipoOperacion { get; set; }
        public required string tipoDocumento { get; set; }
        public required string numeroDocumento { get; set; }
        public required DateTime fecha { get; set; }
        public required string resultado { get; set; }
        public required int codigoError { get; set; }
        public required int idSesionValidacion { get; set; }
    }
}
