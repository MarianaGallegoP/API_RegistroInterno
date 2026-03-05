namespace API_RegistroInterno.Entities
{
    public class Usuario
    {
        public required string tipoDeDocumento { get; set; }
        public required long numeroDocumento { get; set; }
        public required DateTime fechaExpedicion { get; set; }
        public required string nombres { get; set; }
        public required string primerApellido { get; set; }
        public required string segundoApellido { get; set; }
        public required long celular { get; set; }
        public required string correo { get; set; }
    }
}
