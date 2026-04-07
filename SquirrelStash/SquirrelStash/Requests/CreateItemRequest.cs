namespace SquirrelStash.Requests
{
    public record CreateItemRequest(int CategoryId, string? ImageSource, CreatePropertyEntryRequest[] Entries, int WarningThreshold = 5, int CriticalThreshold = 1, 
        int DefaultQuantity = 0, string Note = "");
}
