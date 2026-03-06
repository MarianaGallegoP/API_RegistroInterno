using System.Text.Json.Serialization;

namespace API_RegistroInterno.Models
{
    /// <summary>
    /// Respuesta del endpoint de registro de usuario cliente externo.
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
        public int ExpiresIn { get; set; } = 0;

        [JsonPropertyName("phone_masked")]
        public string PhoneMasked { get; set; } = string.Empty;
    }
}
