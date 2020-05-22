using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class SwitchPicker : BaseWidget
    {
        public int GroupID = 1;
        public int SwitchID = 1;

        public SwitchPicker(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
