using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquirrelStash.DataAccess.Entities
{
    public class PropertyEntry : BaseEntity
    {
        #region Navigation

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int PropertyDefinitionId { get; set; }
        public PropertyDefinition Definition { get; set; }

        #endregion

        public string Value { get; set; }
    }
}
