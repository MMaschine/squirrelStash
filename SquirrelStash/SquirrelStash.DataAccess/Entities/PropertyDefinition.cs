namespace SquirrelStash.DataAccess.Entities
{
    public class PropertyDefinition : BaseEntity
    {
        #region Navigation

        public int CategoryId { get; set; }
        public Category Category;

        #endregion

        public int TypeCode { get; set; }

        public string Name { get; set; } = null!;

        /// <summary>
        /// Values we use to show in Property construction combobox
        /// It should be a string with values separated by commas  
        /// </summary>
        public string? AllowedValues { get; set; }
    }
}
