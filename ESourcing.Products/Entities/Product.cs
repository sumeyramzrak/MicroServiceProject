using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//Monggodb data annotations anlayabilmesi adına yüklendi.
namespace ESourcing.Products.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] //Idnin uniq olmasını sağlar. 
        public string Id { get; set; } //Mongoda id Bson ID olarak gösterebilmek adına string olarak tanımladık.
        [BsonElement("Name")] //display özelliği-Belirtilen şekilde görünmesini sağlayacak
        public string Name { get; set; }
        public string Category  { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
    }
}
