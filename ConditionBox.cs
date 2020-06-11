using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class ConditionBox : BaseWidget
    {
        public dynamic Conditions = new List<object>();

        public BasicCallback OnConditionsChanged;

        public bool AwaitOpen = false;

        public ConditionBox(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }

        public void Open()
        {
            AwaitOpen = true;
        }
    }
}
