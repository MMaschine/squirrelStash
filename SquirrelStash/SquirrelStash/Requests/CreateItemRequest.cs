namespace SquirrelStash.Requests
{
    public record EditItemRequest(
        int CategoryId,
        string? ImageSource,
        CreatePropertyEntryRequest[] Entries,
        int? ItemId = null,
        int WarningThreshold = 5,
        int CriticalThreshold = 1,
        int DefaultQuantity = 0,
        string Note = "")
    {
        public bool IsEdit => ItemId != null;
    }
}
