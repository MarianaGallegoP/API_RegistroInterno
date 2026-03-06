using System.Text.Json.Serialization;

namespace API_RegistroInterno.Models
{
    /// <summary>
    /// Respuesta del endpoint de registro de usuario cliente externo.
    /// </summary>
    public class RegistrarUsuarioClienteExternoResponse
    {
        [JsonPropertyName("exito")]
        public bool Exito { get; set; }

        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        [JsonPropertyName("codigo")]
        public string Codigo { get; set; } = string.Empty;
    }
}
