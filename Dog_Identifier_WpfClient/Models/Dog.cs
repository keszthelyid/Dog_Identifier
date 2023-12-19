using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dog_Identifier_WpfClient.Models
{
    public class Dog
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Link { get; set; }
        public string? ApiId { get; set; }
        public double? Percentage { get; set; }
        public byte[]? PhotoData { get; set; }

    }
}
