using API_RegistroInterno.Models.IdentityValidation;

namespace API_RegistroInterno.Services
{
    /// <summary>
    /// Servicio para llamar a la plataforma interna de validación de identidad (backend to backend).
    /// </summary>
    public interface IIdentityValidationService
    {
        /// <summary>
        /// Envía los datos del usuario a la plataforma interna y obtiene la respuesta (OTP, QUESTIONS o error).
        /// </summary>
        Task<IdentityValidationResponse?> ValidateIdentityAsync(IdentityValidationRequest request, CancellationToken cancellationToken = default);
    }
}
