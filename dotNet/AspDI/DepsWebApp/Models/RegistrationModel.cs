using System.Text.Json.Serialization;

namespace DepsWebApp.Models
{
    /// <summary>
    /// Registration model
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// Login for registration
        /// </summary>
        [JsonPropertyName("login")]
        public string Login { get; set; }

        /// <summary>
        /// Password for registration
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}