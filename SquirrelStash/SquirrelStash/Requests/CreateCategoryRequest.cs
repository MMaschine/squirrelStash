namespace SquirrelStash.Requests
{
    public record EditCategoryRequest(string Title, CreatePropertyRequest[]? Properties, int? CategoryId = null, int[]? PropertiesToRemove = null )
    {
        public bool IsEdit => CategoryId != null;
    }
}
