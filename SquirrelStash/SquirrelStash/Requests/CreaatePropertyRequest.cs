using SquirrelStash.Enums;

namespace SquirrelStash.Requests
{
    public record CreatePropertyRequest(string Name, PropertyTypes Type, string? AllowedValues = "", int? Id = null)
    {
        public bool IsNew = Id == null;
    }
}
