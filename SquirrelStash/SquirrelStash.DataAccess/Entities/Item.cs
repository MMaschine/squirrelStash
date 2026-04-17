namespace SquirrelStash.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Item : BaseEntity
    {
        #region Navigation

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public List<PropertyEntry> PropertyEntries { get; set; } = [];

        #endregion

        [MaxLength(1024)]
        public string? ImageSource { get; set; }

        public int WarningThreshold { get; set; }

        public int CriticalThreshold { get; set; }

        public int Quantity { get; set; }

        [MaxLength(1000)]
        public string? Note { get; set; }
    }
}
