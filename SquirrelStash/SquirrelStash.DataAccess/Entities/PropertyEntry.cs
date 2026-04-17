namespace SquirrelStash.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class PropertyEntry : BaseEntity
    {
        #region Navigation

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int PropertyDefinitionId { get; set; }
        public PropertyDefinition Definition { get; set; }

        #endregion

        [MaxLength(255)]
        public string Value { get; set; }
    }
}
