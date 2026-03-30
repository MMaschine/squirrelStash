namespace SquirrelStash.DataAccess.Entities
{
    public class Item : BaseEntity
    {
        #region Navigation

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public List<PropertyEntry> PropertyEntries { get; set; } = [];

        #endregion

        public string? ImageSource { get; set; }

        public int WarningThreshold { get; set; }

        public int CriticalThreshold { get; set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }
    }
}
