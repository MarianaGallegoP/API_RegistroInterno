using System.Text.Json.Serialization;

namespace API_RegistroInterno.Models
{
    /// <summary>
    /// Respuesta del endpoint de registro de usuario cliente externo.
    /// Soporta respuesta OTP (phone_masked, expires_in) o QUESTIONS (questions).
    /// </summary>
    public class RegistrarUsuarioClienteExternoResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("verification_method")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? VerificationMethod { get; set; }

        [JsonPropertyName("registration_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RegistrationToken { get; set; }

        [JsonPropertyName("validation_session_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ValidationSessionId { get; set; }

        [JsonPropertyName("expires_in")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ExpiresIn { get; set; }

        [JsonPropertyName("phone_masked")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PhoneMasked { get; set; }

        [JsonPropertyName("questions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<VerificationQuestion>? Questions { get; set; }

        internal static string EnmascararCelular(string celular)
        {
            if (string.IsNullOrEmpty(celular) || celular.Length < 7)
                return "**********";
            return celular[..3] + "****" + celular[^3..];
        }

        /// <summary>
        /// Devuelve las preguntas de verificación para el método QUESTIONS.
        /// </summary>
        internal static List<VerificationQuestion> ObtenerPreguntasCuestionario()
        {
            return new List<VerificationQuestion>
            {
                new VerificationQuestion
                {
                    Id = "Q1",
                    Question = "¿En qué ciudad nació?",
                    Options = new List<VerificationQuestionOption>
                    {
                        new() { Id = "A", Text = "Bogotá" },
                        new() { Id = "B", Text = "Medellín" },
                        new() { Id = "C", Text = "Cali" },
                        new() { Id = "D", Text = "Barranquilla" }
                    }
                },
                new VerificationQuestion
                {
                    Id = "Q2",
                    Question = "¿Cuál fue su primer trabajo?",
                    Options = new List<VerificationQuestionOption>
                    {
                        new() { Id = "A", Text = "Empresa privada" },
                        new() { Id = "B", Text = "Sector público" },
                        new() { Id = "C", Text = "Independiente" },
                        new() { Id = "D", Text = "Otro" }
                    }
                }
            };
        }
    }
}
