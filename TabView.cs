using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class TabView : BaseWidget
    {
        public List<Container> TabContainers = new List<Container>();
        public List<string> TabNames = new List<string>();
        public int XOffset = 4;

        public TabView(string UniqueID, BaseWidget Parent = null) : base(UniqueID, Parent)
        {

        }

        public Container NewTab(string Name)
        {
            this.TabNames.Add(Name);
            Container c = new Container(this.UniqueID + "." + this.TabContainers.Count.ToString(), this);
            this.TabContainers.Add(c);
            return c;
        }

        public Container GetTab(string Name)
        {
            if (!TabNames.Contains(Name)) return null;
            return TabContainers[TabNames.IndexOf(Name)];
        }

        public Container GetTab(int Index)
        {
            if (Index >= TabContainers.Count || Index < 0) return null;
            return TabContainers[Index];
        }
    }
}
