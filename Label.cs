using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class Label : BaseWidget
    {
        public string Text;
        public bool Parse = false;
        public Color Color;

        public Label(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }

    public class MultilineLabel : Label
    {
        public MultilineLabel(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
