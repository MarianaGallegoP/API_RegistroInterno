using System.Text.Json.Serialization;

namespace API_RegistroInterno.Models
{
    /// <summary>
    /// Modelo de petición para registrar/validar usuario cliente externo (envío desde la página web).
    /// </summary>
    public class RegistrarUsuarioClienteExternoRequest
    {
        [JsonPropertyName("tipo_documento")]
        public string TipoDocumento { get; set; } = string.Empty;

        [JsonPropertyName("numero_documento")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [JsonPropertyName("fecha_expedicion")]
        public string FechaExpedicion { get; set; } = string.Empty;

        [JsonPropertyName("nombres")]
        public string Nombres { get; set; } = string.Empty;

        [JsonPropertyName("primer_apellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [JsonPropertyName("segundo_apellido")]
        public string SegundoApellido { get; set; } = string.Empty;

        [JsonPropertyName("celular")]
        public string Celular { get; set; } = string.Empty;

        [JsonPropertyName("correo")]
        public string Correo { get; set; } = string.Empty;
    }
}
