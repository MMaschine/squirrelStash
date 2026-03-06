namespace SquirrelStash.DataAccess.Entities
{
    public class Category : BaseEntity
    {
        public int? ParentId { get; set; }
        public Category? Parent { get; set; }
        
        public string Title { get; set; }

        public List<Item> Items { get; set; } = [];

        public List<Category> Subcategories { get; set; } = [];
    }
}
