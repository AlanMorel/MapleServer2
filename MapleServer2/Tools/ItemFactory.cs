using Maple2.Data.Types.Items;
using System;

namespace MapleServer2.Tools {
    // This is needed to create an instance of a item since we inject data from xml
    // xml data is not available in this project.
    public abstract class ItemFactory
    {
        public abstract Item Init(int id);

        internal static Item Create(int id)
        {
            throw new NotImplementedException();
        }
    }
}
