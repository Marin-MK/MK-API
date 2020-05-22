using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class RadioButton : BaseWidget
    {
        public bool Selected = false;
        public string Text;

        public BasicCallback OnSelected;

        public RadioButton(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
