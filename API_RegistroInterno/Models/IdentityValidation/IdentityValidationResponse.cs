using System.Text.Json.Serialization;
using API_RegistroInterno.Models;

namespace API_RegistroInterno.Models.IdentityValidation
{
    /// <summary>
    /// Respuesta de la plataforma interna de validación de identidad.
    /// Puede ser VALIDATION_SUCCESS_OTP, VALIDATION_SUCCESS_QUESTIONS, o códigos de error.
    /// </summary>
    public class IdentityValidationResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("validation_session_id")]
        public string? ValidationSessionId { get; set; }

        [JsonPropertyName("verification_method")]
        public string? VerificationMethod { get; set; }

        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonPropertyName("phone_masked")]
        public string? PhoneMasked { get; set; }

        [JsonPropertyName("questions")]
        public List<VerificationQuestion>? Questions { get; set; }
    }
}
