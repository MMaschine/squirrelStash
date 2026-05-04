namespace SquirrelStash.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class PropertyDefinition : BaseEntity
    {
        #region Navigation

        public int CategoryId { get; set; }

        public Category Category;

        public List<PropertyEntry> Entries { get; set; }

        #endregion

        public int TypeCode { get; set; }

        [MaxLength(80)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Values we use to show in Property construction combobox
        /// It should be a string with values separated by commas  
        /// </summary>
        [MaxLength(1000)]
        public string? AllowedValues { get; set; }
    }
}
