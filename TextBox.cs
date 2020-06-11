using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class TextBox : BaseWidget
    {
        public string Text;
        public int Index = -1;
        public bool Focus = false;

        public BasicCallback OnTextChanged;

        public TextBox(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }

    public class MultilineTextBox : TextBox
    {
        public MultilineTextBox(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
