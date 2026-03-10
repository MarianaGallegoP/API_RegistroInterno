using System.Text.Json.Serialization;

namespace API_RegistroInterno.Models.IdentityValidation
{
    /// <summary>
    /// Payload enviado a la plataforma interna de validación de identidad (backend to backend).
    /// </summary>
    public class IdentityValidationRequest
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
