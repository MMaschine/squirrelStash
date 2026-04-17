using SquirrelStash.Enums;

namespace SquirrelStash.Requests
{
    public record CreatePropertyRequest(string Name, PropertyTypes Type, string? AllowedValues = "")
    {
    }
}
