using System.Text.Json.Serialization;

namespace API_RegistroInterno.Models
{
    /// <summary>
    /// Opción de respuesta para una pregunta de verificación.
    /// </summary>
    public class VerificationQuestionOption
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
