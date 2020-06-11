using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class CheckBox : BaseWidget
    {
        public bool Checked = false;
        public string Text = "CheckBox";

        public BasicCallback OnCheckChanged;

        public CheckBox(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
