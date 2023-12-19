using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Dog_Identifier.Models
{
    public class Dog
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string? Name { get; set; }

        [BsonElement("link"), BsonRepresentation(BsonType.String)]
        public string? Link { get; set; }

        [BsonElement("ApiId"), BsonRepresentation(BsonType.String)]
        public string? ApiId { get; set; }

        [BsonIgnore]
        public double? Percentage { get; set; }
        [BsonIgnore]
        public byte[]? PhotoData { get; set; }

        public Dog GetCopy()
        {
            Dog newDog = new Dog();

            newDog.Id = Id;
            newDog.ApiId = ApiId;
            newDog.Name = Name;
            newDog.Link = Link;
            newDog.Percentage = 0;
            newDog.PhotoData = null;

            return newDog;
        }
    }
}
