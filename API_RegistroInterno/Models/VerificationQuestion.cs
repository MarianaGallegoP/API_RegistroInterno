using System.Text.Json.Serialization;

namespace API_RegistroInterno.Models
{
    /// <summary>
    /// Pregunta de verificación para el método QUESTIONS.
    /// </summary>
    public class VerificationQuestion
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;

        [JsonPropertyName("options")]
        public List<VerificationQuestionOption> Options { get; set; } = new();
    }
}
