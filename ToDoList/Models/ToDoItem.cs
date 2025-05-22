using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoList.Models
{
    public class ToDoItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("isCompleted")]
        public bool IsCompleted { get; set; } = false;

        [BsonElement("deadline")]
        public DateTime Deadline { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; } = "";

    }
}
