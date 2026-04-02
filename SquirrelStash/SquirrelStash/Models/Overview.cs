namespace SquirrelStash.Models
{
    public class Overview
    {
        public int TotalCategoriesCount { get; set; }

        public int TotalItemsCount { get; set; }

        public int WarningThresholdsReachedCount => ItemsToHighlight.Count(x=>!x.IsCritical);

        public int CriticalThresholdsReachedCount => ItemsToHighlight.Count(x => x.IsCritical);

        public List<OverviewItem> ItemsToHighlight { get; set; } = [];
    }
}
