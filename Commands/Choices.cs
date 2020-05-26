using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI.Commands
{
    public class Choices : Command
    {
        public Choices() : base("choice", "Show Choices")
        {
            this.OnCreateWindow.Add(CreateWindow);
            this.OnSaveWindow.Add(SaveWindow);
            this.OnCreateBlank.Add(CreateBlank);
            this.HeaderColor = HeaderColors.GREEN;
            this.PickerTabName = "General";
            this.HasBranches = true;
            this.BranchIdentifier = "choice_branch";
        }

        public List<BaseWidget> CreateWindow(dynamic Utility)
        {
            throw new NotImplementedException();
        }

        public void SaveWindow(dynamic Utility)
        {
            throw new NotImplementedException();
        }

        public void CreateBlank(dynamic Utility)
        {
            Utility.CreateParam(":choices", new List<object>());
        }
    }
}
