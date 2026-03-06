namespace SquirrelStash.DataAccess.Entities
{
    public class Item : BaseEntity
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Name { get; set; }

        public string? ImageSource { get; set; }

        public int WarningThreshold { get; set; }

        public int CriticalThreshold { get; set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }
    }
}
