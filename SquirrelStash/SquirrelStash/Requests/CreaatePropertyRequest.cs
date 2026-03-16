using SquirrelStash.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquirrelStash.Requests
{
    public record CreatePropertyRequest(string Name, PropertyTypes Type, string? AllowedValues = "")
    {
    }
}
