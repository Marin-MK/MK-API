using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public abstract class BaseWidget
    {
        public string UniqueID { get; }
        public BaseWidget Parent;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public bool Enabled = true;

        public delegate List<BaseWidget> BasicCallback();

        public BaseWidget(string UniqueID, BaseWidget Parent = null)
        {
            this.UniqueID = UniqueID;
            this.Parent = Parent;
            this.X = 0;
            this.Y = 0;
            this.Width = -1;
            this.Height = -1;
        }

        public virtual BaseWidget Clone()
        {
            throw new MissingMemberException();
        }
    }
}
