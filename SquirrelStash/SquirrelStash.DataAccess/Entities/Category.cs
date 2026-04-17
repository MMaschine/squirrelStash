namespace SquirrelStash.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Category : BaseEntity
    {
        #region Navigation

        public List<PropertyDefinition> Properties { get; set; } = [];

        public List<Item> Items { get; set; } = [];

        #endregion

        [MaxLength(120)]
        public string Title { get; set; }
    }
}
