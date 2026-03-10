using System.Net.Http.Json;
using System.Text.Json;
using API_RegistroInterno.Models.IdentityValidation;
using Microsoft.Extensions.Options;

namespace API_RegistroInterno.Services
{
    /// <summary>
    /// Implementación que llama por POST a la plataforma interna de validación de identidad.
    /// </summary>
    public class IdentityValidationService : IIdentityValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly IdentityValidationOptions _options;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public IdentityValidationService(HttpClient httpClient, IOptions<IdentityValidationOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
            if (!string.IsNullOrEmpty(_options.BaseUrl))
                _httpClient.BaseAddress = new Uri(_options.BaseUrl.TrimEnd('/'));
        }

        public async Task<IdentityValidationResponse?> ValidateIdentityAsync(IdentityValidationRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_options.BaseUrl))
                return null;

            var endpoint = _options.ValidateEndpoint?.TrimStart('/') ?? "validate";
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, request, JsonOptions, cancellationToken).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    // Intentar deserializar como respuesta de error con status
                    try
                    {
                        var errorBody = JsonSerializer.Deserialize<IdentityValidationResponse>(content, JsonOptions);
                        if (!string.IsNullOrEmpty(errorBody?.Status))
                            return errorBody;
                    }
                    catch { /* ignorar */ }
                    return new IdentityValidationResponse { Status = "SERVICE_ERROR" };
                }

                return JsonSerializer.Deserialize<IdentityValidationResponse>(content, JsonOptions);
            }
            catch (HttpRequestException)
            {
                return new IdentityValidationResponse { Status = "SERVICE_ERROR" };
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                return new IdentityValidationResponse { Status = "SERVICE_ERROR" };
            }
        }
    }

    /// <summary>
    /// Opciones de configuración para la plataforma interna de validación de identidad.
    /// </summary>
    public class IdentityValidationOptions
    {
        public const string SectionName = "IdentityValidation";

        /// <summary>
        /// URL base del servicio de validación (ej: https://api-interna.ejemplo.com/identity).
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Ruta del endpoint de validación (ej: /validate). Se concatena a BaseUrl.
        /// </summary>
        public string ValidateEndpoint { get; set; } = "/validate";
    }
}
