using System;
using System.Collections.Generic;
using System.Linq;

namespace MKAPI
{
    public class Command
    {
        public delegate List<BaseWidget> WidgetCallback();
        public delegate void CloneCallback(Command Command);
        public delegate void UtilityCallback(dynamic Utility);
        public delegate List<BaseWidget> LoadCallback(dynamic Utility);

        public string Identifier;
        public string Name;
        public bool ShowHeader;
        public Color HeaderColor;
        public int Width = -1;
        public int Height = -1;
        public List<Color> TextColors;

        public int WindowWidth = -1;
        public int WindowHeight = -1;

        public List<CloneCallback> OnCreateEmptyClone = new List<CloneCallback>();
        public List<WidgetCallback> OnCreateReadOnly = new List<WidgetCallback>();
        public List<LoadCallback> OnLoadReadOnly = new List<LoadCallback>();
        public List<LoadCallback> OnCreateWindow = new List<LoadCallback>();
        public List<UtilityCallback> OnSaveWindow = new List<UtilityCallback>();

        public Command(string Identifier, string Name, Color HeaderColor = null)
        {
            this.Identifier = Identifier;
            this.Name = Name;
            this.ShowHeader = true;
            this.HeaderColor = HeaderColor == null ? HeaderColors.WHITE : HeaderColor;
            this.Width = -1;
            this.Height = -1;
            this.TextColors = new List<Color>() { Color.WHITE };
            this.OnCreateEmptyClone.Add(delegate (Command c)
            {
                c.ShowHeader = this.ShowHeader;
                c.HeaderColor = this.HeaderColor.Clone();
                c.Width = this.Width;
                c.Height = this.Height;
                c.WindowWidth = this.WindowWidth;
                c.WindowHeight = this.WindowHeight;
                c.TextColors = new List<Color>(this.TextColors);
                c.OnCreateEmptyClone = new List<CloneCallback>(this.OnCreateEmptyClone);
                c.OnCreateReadOnly = new List<WidgetCallback>(this.OnCreateReadOnly);
                c.OnLoadReadOnly = new List<LoadCallback>(this.OnLoadReadOnly);
                c.OnCreateWindow = new List<LoadCallback>(this.OnCreateWindow);
                c.OnSaveWindow = new List<UtilityCallback>(this.OnSaveWindow);
            });
        }

        protected List<BaseWidget> Refresh(params BaseWidget[] Widgets)
        {
            return Widgets.ToList();
        }

        public Command CreateEmptyClone()
        {
            Command c = new Command(this.Identifier, this.Name);
            this.OnCreateEmptyClone.ForEach(d => d(c));
            return c;
        }

        public List<BaseWidget> CallCreateReadOnly()
        {
            List<BaseWidget> Widgets = new List<BaseWidget>();
            this.OnCreateReadOnly.ForEach(d =>
            {
                Widgets.AddRange(d());
            });
            return Widgets;
        }

        public List<BaseWidget> CallLoadReadOnly(dynamic Utility)
        {
            List<BaseWidget> Widgets = new List<BaseWidget>();
            this.OnLoadReadOnly.ForEach(d =>
            {
                Widgets.AddRange(d(Utility));
            });
            return Widgets;
        }

        public List<BaseWidget> CallCreateWindow(dynamic Utility)
        {
            List<BaseWidget> Widgets = new List<BaseWidget>();
            this.OnCreateWindow.ForEach(d =>
            {
                Widgets.AddRange(d(Utility));
            });
            return Widgets;
        }

        public void CallSaveWindow(dynamic Utility)
        {
            this.OnSaveWindow.ForEach(d =>
            {
                d(Utility);
            });
        }
    }
}
