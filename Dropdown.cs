using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class Dropdown : BaseWidget
    {
        public int Index = 0;
        public List<string> Items;

        public BasicCallback OnSelectionChanged;

        public Dropdown(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
