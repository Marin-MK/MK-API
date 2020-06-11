using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class Button : BaseWidget
    {
        public string Text = "Button";

        public BasicCallback OnPressed;

        public Button(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
