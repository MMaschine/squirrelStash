namespace SquirrelStash.DataAccess.Entities
{
    public class PropertyDefinition : BaseEntity
    {
        #region Navigation

        public int CategoryId { get; set; }
        public Category Category;

        #endregion

        public int TypeCode { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Values we use to 1. Show in Property construction combobox, 2. Define sort order.
        /// It should be a string with values separated by commas  
        /// </summary>
        public string? AllowedValues { get; set; }
    }
}
