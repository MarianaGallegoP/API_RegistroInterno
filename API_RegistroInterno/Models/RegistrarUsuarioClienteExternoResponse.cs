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
        public string VerificationMethod { get; set; } = string.Empty;

        [JsonPropertyName("registration_token")]
        public string RegistrationToken { get; set; } = string.Empty;

        [JsonPropertyName("validation_session_id")]
        public string ValidationSessionId { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ExpiresIn { get; set; }

        [JsonPropertyName("phone_masked")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PhoneMasked { get; set; }

        [JsonPropertyName("questions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<VerificationQuestion>? Questions { get; set; }
    }
}
