namespace SquirrelStash.DataAccess.Entities
{
    public class Category : BaseEntity
    {
        #region Navigation

        public List<PropertyDefinition> Properties { get; set; } = [];

        public List<Item> Items { get; set; } = [];

        #endregion

        public string Title { get; set; }
    }
}
