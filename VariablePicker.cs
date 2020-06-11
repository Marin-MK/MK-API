using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class VariablePicker : BaseWidget
    {
        public int GroupID = 1;
        public int VariableID = 1;

        public VariablePicker(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }
    }
}
