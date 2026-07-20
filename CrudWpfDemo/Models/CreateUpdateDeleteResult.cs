using System.Text.Json.Serialization;

namespace CrudWpfDemo.Models
{
    public class CreateUpdateDeleteResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("id")]
        [JsonConverter(typeof(FlexibleIntConverter))]
        public int? Id { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
