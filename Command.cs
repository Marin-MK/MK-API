using System;
using System.Collections;
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
        public delegate void ChoiceCallback(int ChoiceIndex);
        public delegate dynamic BranchCallback(int BranchIndex, int CommandIndent);

        public string Identifier;
        public string Name;
        public bool ShowHeader;
        public Color HeaderColor;
        public int Width = -1;
        public int Height = -1;
        public List<Color> TextColors;

        public bool HasBranches = false;
        public string BranchIdentifier;
        public bool IsDeletable = true;
        public bool IsEditable = true;

        public string PickerTabName;

        public int WindowWidth = -1;
        public int WindowHeight = -1;

        public List<CloneCallback> OnCreateEmptyClone = new List<CloneCallback>();
        public List<WidgetCallback> OnCreateReadOnly = new List<WidgetCallback>();
        public List<LoadCallback> OnLoadReadOnly = new List<LoadCallback>();
        public List<LoadCallback> OnCreateWindow = new List<LoadCallback>();
        public List<UtilityCallback> OnSaveWindow = new List<UtilityCallback>();
        public List<UtilityCallback> OnCreateBlank = new List<UtilityCallback>();

        public Command(string Identifier, string Name)
        {
            this.Identifier = Identifier;
            this.Name = Name;
            this.ShowHeader = true;
            this.HeaderColor = HeaderColors.WHITE;
            this.Width = -1;
            this.Height = -1;
            this.TextColors = new List<Color>() { Color.WHITE };
            this.OnCreateEmptyClone.Add(delegate (Command c)
            {
                c.PickerTabName = this.PickerTabName;
                c.ShowHeader = this.ShowHeader;
                c.HeaderColor = this.HeaderColor.Clone();
                c.Width = this.Width;
                c.Height = this.Height;
                c.WindowWidth = this.WindowWidth;
                c.WindowHeight = this.WindowHeight;
                c.TextColors = new List<Color>(this.TextColors);
                c.HasBranches = this.HasBranches;
                c.BranchIdentifier = this.BranchIdentifier;
                c.IsDeletable = this.IsDeletable;
                c.IsEditable = this.IsEditable;
                c.OnCreateEmptyClone = new List<CloneCallback>(this.OnCreateEmptyClone);
                c.OnCreateReadOnly = new List<WidgetCallback>(this.OnCreateReadOnly);
                c.OnLoadReadOnly = new List<LoadCallback>(this.OnLoadReadOnly);
                c.OnCreateWindow = new List<LoadCallback>(this.OnCreateWindow);
                c.OnSaveWindow = new List<UtilityCallback>(this.OnSaveWindow);
                c.OnCreateBlank = new List<UtilityCallback>(this.OnCreateBlank);
            });
        }

        protected List<BaseWidget> Refresh(params object[] Widgets)
        {
            List<BaseWidget> RefreshWidgets = new List<BaseWidget>();
            foreach (object o in Widgets)
            {
                if (o is BaseWidget)
                {
                    if (RefreshWidgets.Contains((BaseWidget)o)) continue;
                    RefreshWidgets.Add((BaseWidget) o);
                }
                else if (o is IList)
                {
                    foreach (object widget in (IList) o)
                    {
                        if (RefreshWidgets.Contains((BaseWidget) widget)) continue;
                        RefreshWidgets.Add((BaseWidget) widget);
                    }
                }
            }
            return RefreshWidgets;
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
            this.OnSaveWindow.ForEach(d => d(Utility));
        }

        public void CallCreateBlank(dynamic Utility)
        {
            this.OnCreateBlank.ForEach(d => d(Utility));
        }
    }

    public enum ButtonType
    {
        OK = 0,
        OKCancel = 1,
        YesNo = 2,
        YesNoCancel = 3
    }

    public enum IconType
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Info = 3
    }
}
