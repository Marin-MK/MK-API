using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI.Commands
{
    public class ChoiceBranch : Command
    {
        Label ChoiceLabel;

        public ChoiceBranch() : base("choice_branch", null)
        {
            this.OnCreateReadOnly.Add(CreateReadOnly);
            this.OnLoadReadOnly.Add(LoadReadOnly);
            this.HeaderColor = HeaderColors.GREEN;
            this.ShowHeader = false;
            this.IsDeletable = false;
            this.IsEditable = false;
        }

        public List<BaseWidget> CreateReadOnly()
        {
            ChoiceLabel = new Label("choice");
            ChoiceLabel.Color = Color.YELLOW;
            return Refresh(ChoiceLabel);
        }

        public List<BaseWidget> LoadReadOnly(dynamic Utility)
        {
            int idx = Utility.ParamAsInt("choice_index");
            dynamic ParentChoice = Utility.GetBranchParent();
            ChoiceLabel.Text = $"Chose [{ParentChoice.ParamAsArray("choices")[idx]}]:";
            return Refresh(ChoiceLabel);
        }
    }
}
