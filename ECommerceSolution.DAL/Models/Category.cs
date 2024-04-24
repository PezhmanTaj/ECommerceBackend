using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ParentCategoryId { get; set; }
    public List<Category> Subcategories { get; set; }
    public string ImagePath { get; set; }
    public string SEOKeywords { get; set; }
    public bool IsActive { get; set; }

    public Category()
    {
        Subcategories = new List<Category>();
    }
}
