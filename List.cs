using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class List : BaseWidget
    {
        public List<string> Items = new List<string>();
        public int Index = -1;

        public BasicCallback OnSelectionChanged;

        public List(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
