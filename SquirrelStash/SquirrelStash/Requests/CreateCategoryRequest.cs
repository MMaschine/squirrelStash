using SquirrelStash.DataAccess.Entities;

namespace SquirrelStash.Requests
{
    public record CreateCategoryRequest(string Title, CreatePropertyRequest[]? Properties)
    {
    }
}
