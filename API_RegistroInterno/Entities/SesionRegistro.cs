namespace API_RegistroInterno.Entities
{
    public class SesionRegistro
    {
        public required int idSesion { get; set; }
        public required int idUsuario { get; set; }
        public required string metodoVerificacion { get; set; }
        public required int sesionValidacionInterna { get; set; }
        public required string estado { get; set; }
        public required DateTime expiracion { get; set; }
        public required int contadorIntentos { get; set; }
    }
}
