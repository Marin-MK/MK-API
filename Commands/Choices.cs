using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKAPI.Commands
{
    public class Choices : Command
    {
        TabView TabView;
        
        Container SimpleContainer;
        MultilineLabel SimpleTooAdvancedLabel;
        List<Label> SimpleChoiceLabels;
        List<TextBox> SimpleChoiceTextBoxes;
        List<RadioButton> SimpleChoiceButtons;
        Label SimpleCancelLabel;
        RadioButton SimpleDisallowButton;
        RadioButton SimpleBranchButton;
        
        Container AdvContainer;
        Label AdvChoiceLabel;
        List AdvList;
        Button AdvAddButton;
        Button AdvRemoveButton;
        Label AdvEntryTextLabel;
        TextBox AdvEntryTextBox;
        CheckBox AdvShowIfButton;
        ConditionBox AdvConditions;
        Button AdvEditConditionsButton;
        Label AdvCancelLabel;
        RadioButton AdvDisallowButton;
        RadioButton AdvBranchButton;
        RadioButton AdvTreatAsChoiceButton;
        Dropdown AdvTreatAsChoiceBox;
        Label AdvOutcomeLabel;
        CheckBox AdvBranchCommandButton;
        CheckBox AdvStoreInVariableButton;
        VariablePicker AdvVariablePicker;

        List<string> Text;
        List<dynamic> Conditions;
        bool DisallowCancel;
        bool BranchCancel;
        int ChoiceCancel;
                
        public Choices() : base("choice", "Show Choices")
        {
            this.OnCreateWindow.Add(CreateWindow);
            this.OnSaveWindow.Add(SaveWindow);
            this.OnCreateBlank.Add(CreateBlank);
            this.HeaderColor = HeaderColors.GREEN;
            this.PickerTabName = "General";
            this.HasBranches = true;
            this.BranchIdentifier = "choice_branch";
            this.WindowWidth = 347;
            this.WindowHeight = 370;
        }

        public List<BaseWidget> CreateWindow(dynamic Utility)
        {
            this.Text = new List<string>();
            foreach (object o in Utility.ParamAsArray("choices")) this.Text.Add((string) o);
            if (this.Text.Count < 8) this.Text.AddRange(new string[8 - this.Text.Count]);
            this.Conditions = Utility.HasParam("conditions") ? Utility.ParamAsArray("conditions") : new List<dynamic>();
            DisallowCancel = Utility.ParamIsString("cancel") && Utility.ParamAsString("cancel") == ":disallow";
            BranchCancel = Utility.ParamIsString("cancel") && Utility.ParamAsString("cancel") == ":branch";
            ChoiceCancel = Utility.ParamIsInt("cancel") ? Utility.ParamAsInt("cancel") : -1;

            TabView = new TabView("tabview");
            TabView.X = 1;
            TabView.Y = 24;
            TabView.Width = 345;
            TabView.Height = 306;
            SimpleContainer = TabView.NewTab("Simple");

            SimpleTooAdvancedLabel = new MultilineLabel("tooadvanced", SimpleContainer);
            SimpleTooAdvancedLabel.X = 30;
            SimpleTooAdvancedLabel.Y = 105;
            SimpleTooAdvancedLabel.Width = 285;
            SimpleTooAdvancedLabel.Text = "There are active options in the Advanced tab. Please disable the advanced features, or use the Advanced tab.";
            SimpleTooAdvancedLabel.Visible = false;
            SimpleCancelLabel = new Label("simplecancellabel", SimpleContainer);
            SimpleCancelLabel.X = 252;
            SimpleCancelLabel.Y = 4;
            SimpleCancelLabel.Text = "On Cancel:";
            SimpleDisallowButton = new RadioButton("simpledisallow", SimpleContainer);
            SimpleDisallowButton.X = 252;
            SimpleDisallowButton.Y = 21;
            SimpleDisallowButton.Text = "Disallow";
            SimpleDisallowButton.OnSelectionChanged += delegate ()
            {
                if (SimpleDisallowButton.Selected)
                {
                    DisallowCancel = true;
                    BranchCancel = false;
                    ChoiceCancel = -1;
                    return Sync(Utility);
                }
                return Refresh();
            };
            SimpleBranchButton = new RadioButton("simplebranch", SimpleContainer);
            SimpleBranchButton.X = 252;
            SimpleBranchButton.Y = 41;
            SimpleBranchButton.Text = "Branch";
            SimpleBranchButton.OnSelectionChanged += delegate ()
            {
                if (SimpleBranchButton.Selected)
                {
                    DisallowCancel = false;
                    BranchCancel = true;
                    ChoiceCancel = -1;
                    return Sync(Utility);
                }
                return Refresh();
            };

            this.SimpleChoiceLabels = new List<Label>();
            this.SimpleChoiceTextBoxes = new List<TextBox>();
            this.SimpleChoiceButtons = new List<RadioButton>();

            List<object> choices = Utility.ParamAsArray("choices");
            for (int i = 0; i < 8; i++)
            {
                Label label = new Label($"choice{i + 1}label", SimpleContainer);
                TextBox textbox = new TextBox($"choice{i + 1}text", SimpleContainer);
                RadioButton choice = new RadioButton($"choice{i + 1}button", SimpleContainer);

                label.X = 18;
                label.Y = 14 + 33 * i;
                label.Text = $"Choice {i + 1}:";
                SimpleChoiceLabels.Add(label);
                
                textbox.X = 77;
                textbox.Y = 9 + 33 * i;
                textbox.Width = 150;
                textbox.Height = 27;
                textbox.OnTextChanged += delegate ()
                {
                    choice.Enabled = !string.IsNullOrEmpty(textbox.Text);
                    this.Text[SimpleChoiceTextBoxes.IndexOf(textbox)] = textbox.Text;
                    int index = SimpleChoiceTextBoxes.IndexOf(textbox);
                    if (string.IsNullOrEmpty(textbox.Text))
                    {
                        this.Text.RemoveAt(index);
                        if (index < this.Conditions.Count) this.Conditions.RemoveAt(index);
                    }
                    return Refresh(choice, Sync(Utility));
                };
                SimpleChoiceTextBoxes.Add(textbox);
                
                choice.X = 252;
                choice.Y = 61 + 20 * i;
                choice.Text = $"Choice {i + 1}";
                choice.OnSelectionChanged += delegate ()
                {
                    if (choice.Selected)
                    {
                        DisallowCancel = false;
                        BranchCancel = false;
                        ChoiceCancel = SimpleChoiceButtons.IndexOf(choice);
                        return Sync(Utility);
                    }
                    return Refresh();
                };
                SimpleChoiceButtons.Add(choice);
            }

            AdvContainer = TabView.NewTab("Advanced");
            AdvChoiceLabel = new Label("choiceslabel", AdvContainer);
            AdvChoiceLabel.X = 11;
            AdvChoiceLabel.Text = "Choices:";
            AdvList = new List("choicelist", AdvContainer);
            AdvList.X = 8;
            AdvList.Y = 17;
            AdvList.Width = 141;
            AdvList.Height = 121;
            AdvList.OnSelectionChanged += delegate ()
            {
                return UpdateSelection();
            };
            for (int i = 0; i < choices.Count; i++) { AdvList.Items.Add((string) choices[i]); }
            AdvAddButton = new Button("add", AdvContainer);
            AdvAddButton.X = 4;
            AdvAddButton.Y = 138;
            AdvAddButton.Width = 74;
            AdvAddButton.Height = 31;
            AdvAddButton.Text = "Add";
            AdvRemoveButton = new Button("remove", AdvContainer);
            AdvRemoveButton.X = 78;
            AdvRemoveButton.Y = 138;
            AdvRemoveButton.Width = 74;
            AdvRemoveButton.Height = 31;
            AdvRemoveButton.Text = "Remove";
            AdvEntryTextLabel = new Label("textlabel", AdvContainer);
            AdvEntryTextLabel.X = 157;
            AdvEntryTextLabel.Y = 22;
            AdvEntryTextLabel.Text = "Text:";
            AdvEntryTextLabel.Enabled = false;
            AdvEntryTextBox = new TextBox("textbox", AdvContainer);
            AdvEntryTextBox.X = 187;
            AdvEntryTextBox.Y = 17;
            AdvEntryTextBox.Width = 150;
            AdvEntryTextBox.Height = 27;
            AdvEntryTextBox.Enabled = false;
            AdvEntryTextBox.OnTextChanged += delegate ()
            {
                if (AdvList.Index == -1) return Refresh();
                AdvList.Items[AdvList.Index] = AdvEntryTextBox.Text;
                AdvTreatAsChoiceBox.Items = AdvList.Items;
                this.Text[AdvList.Index] = AdvEntryTextBox.Text;
                return Refresh(AdvList, AdvTreatAsChoiceBox, Sync(Utility));
            };
            AdvShowIfButton = new CheckBox("showif", AdvContainer);
            AdvShowIfButton.X = 158;
            AdvShowIfButton.Y = 49;
            AdvShowIfButton.Text = "Show if:";
            AdvShowIfButton.Enabled = false;
            AdvShowIfButton.OnCheckChanged += delegate ()
            {
                AdvConditions.Enabled = AdvShowIfButton.Checked;
                if (!AdvShowIfButton.Checked)
                {
                    AdvConditions.Conditions = null;
                    if (AdvList.Index < this.Conditions.Count) this.Conditions[AdvList.Index] = null;
                }
                AdvEditConditionsButton.Enabled = AdvShowIfButton.Checked;
                return Refresh(AdvConditions, AdvEditConditionsButton, Sync(Utility));
            };
            AdvConditions = new ConditionBox("conditions", AdvContainer);
            AdvConditions.X = 178;
            AdvConditions.Y = 68;
            AdvConditions.Width = 159;
            AdvConditions.Height = 65;
            AdvConditions.Enabled = false;
            AdvConditions.OnConditionsChanged += delegate ()
            {
                if (AdvConditions.Conditions.Count == 0)
                {
                    AdvShowIfButton.Checked = false;
                    if (AdvList.Index < Conditions.Count) Conditions[AdvList.Index] = null;
                }
                else
                {
                    if (AdvList.Index < Conditions.Count) Conditions[AdvList.Index] = AdvConditions.Conditions;
                    else
                    {
                        for (int i = Conditions.Count; i < AdvList.Index; i++) Conditions.Add(null);
                        Conditions.Add(AdvConditions.Conditions);
                    }
                }
                return Refresh(AdvShowIfButton, Sync(Utility));
            };
            AdvEditConditionsButton = new Button("editconditions", AdvContainer);
            AdvEditConditionsButton.X = 282;
            AdvEditConditionsButton.Y = 133;
            AdvEditConditionsButton.Width = 59;
            AdvEditConditionsButton.Height = 31;
            AdvEditConditionsButton.Text = "Edit";
            AdvEditConditionsButton.Enabled = false;
            AdvEditConditionsButton.OnPressed += delegate ()
            {
                AdvConditions.Open();
                return Refresh();
            };
            AdvCancelLabel = new Label("advcancellabel", AdvContainer);
            AdvCancelLabel.X = 8;
            AdvCancelLabel.Y = 172;
            AdvCancelLabel.Text = "When cancelled:";
            AdvDisallowButton = new RadioButton("advdisallow", AdvContainer);
            AdvDisallowButton.X = 9;
            AdvDisallowButton.Y = 192;
            AdvDisallowButton.Text = "Disallow";
            AdvDisallowButton.OnSelectionChanged += delegate ()
            {
                if (AdvDisallowButton.Selected)
                {
                    DisallowCancel = true;
                    BranchCancel = false;
                    ChoiceCancel = -1;
                    return Sync(Utility);
                }
                return Refresh();
            };
            AdvBranchButton = new RadioButton("advbranch", AdvContainer);
            AdvBranchButton.X = 9;
            AdvBranchButton.Y = 212;
            AdvBranchButton.Text = "Branch";
            AdvBranchButton.OnSelectionChanged += delegate ()
            {
                if (AdvBranchButton.Selected)
                {
                    DisallowCancel = false;
                    BranchCancel = true;
                    ChoiceCancel = -1;
                    return Sync(Utility);
                }
                return Refresh();
            };
            AdvTreatAsChoiceButton = new RadioButton("advtreataschoice", AdvContainer);
            AdvTreatAsChoiceButton.X = 9;
            AdvTreatAsChoiceButton.Y = 232;
            AdvTreatAsChoiceButton.Text = "Treat as choice";
            AdvTreatAsChoiceButton.OnSelectionChanged += delegate ()
            {
                if (AdvTreatAsChoiceButton.Selected)
                {
                    DisallowCancel = false;
                    BranchCancel = false;
                    ChoiceCancel = 0;
                    return Sync(Utility);
                }
                return Refresh();
            };
            AdvTreatAsChoiceBox = new Dropdown("advchoicebox", AdvContainer);
            AdvTreatAsChoiceBox.X = 30;
            AdvTreatAsChoiceBox.Y = 251;
            AdvTreatAsChoiceBox.Width = 123;
            AdvTreatAsChoiceBox.Height = 25;
            AdvTreatAsChoiceBox.Items = AdvList.Items;
            AdvOutcomeLabel = new Label("outcome", AdvContainer);
            AdvOutcomeLabel.X = 172;
            AdvOutcomeLabel.Y = 195;
            AdvOutcomeLabel.Text = "Outcome:";
            AdvBranchCommandButton = new CheckBox("branchcommand", AdvContainer);
            AdvBranchCommandButton.X = 173;
            AdvBranchCommandButton.Y = 212;
            AdvBranchCommandButton.Text = "Branch this command";
            AdvBranchCommandButton.Checked = true;
            AdvBranchCommandButton.OnCheckChanged += delegate ()
            {
                return Sync(Utility);
            };
            if (Utility.HasParam("branch") && !Utility.ParamAsBool("branch")) AdvBranchCommandButton.Checked = false;
            AdvStoreInVariableButton = new CheckBox("storeinvar", AdvContainer);
            AdvStoreInVariableButton.X = 173;
            AdvStoreInVariableButton.Y = 232;
            AdvStoreInVariableButton.Text = "Store in Game Variable";
            if (Utility.HasParam("variable") && Utility.ParamIsHash("variable")) AdvStoreInVariableButton.Checked = true;
            AdvStoreInVariableButton.OnCheckChanged += delegate ()
            {
                AdvVariablePicker.Enabled = AdvStoreInVariableButton.Checked;
                return Sync(Utility);
            };
            AdvVariablePicker = new VariablePicker("variablepicker", AdvContainer);
            AdvVariablePicker.X = 194;
            AdvVariablePicker.Y = 251;
            AdvVariablePicker.Width = 143;
            AdvVariablePicker.Height = 25;
            AdvVariablePicker.Enabled = false;
            if (AdvStoreInVariableButton.Checked)
            {
                AdvVariablePicker.GroupID = (int) Utility.ParamAsHash("variable")[":group_id"];
                AdvVariablePicker.VariableID = (int) Utility.ParamAsHash("variable")[":variable_id"];
                AdvVariablePicker.Enabled = true;
            }

            return Refresh(TabView,
                SimpleContainer,
                SimpleTooAdvancedLabel, SimpleCancelLabel, SimpleDisallowButton, SimpleBranchButton, SimpleChoiceLabels, SimpleChoiceTextBoxes, SimpleChoiceButtons,
                AdvContainer,
                AdvChoiceLabel, AdvAddButton, AdvRemoveButton, AdvEntryTextLabel, AdvShowIfButton, AdvConditions, AdvEditConditionsButton,
                AdvCancelLabel, AdvOutcomeLabel, AdvBranchCommandButton, AdvStoreInVariableButton, AdvVariablePicker, Sync(Utility));
        }

        public List<BaseWidget> UpdateSelection()
        {
            AdvEntryTextLabel.Enabled = AdvList.Index != -1;
            AdvEntryTextBox.Enabled = AdvList.Index != -1;
            AdvShowIfButton.Enabled = AdvList.Index != -1;
            AdvShowIfButton.Checked = false;
            AdvConditions.Enabled = false;
            AdvConditions.Conditions = null;
            AdvEditConditionsButton.Enabled = false;
            if (AdvList.Index != -1)
            {
                AdvEntryTextBox.Text = AdvList.Items[AdvList.Index];
                dynamic Conditions = AdvList.Index < this.Conditions.Count ? this.Conditions[AdvList.Index] : null;
                if (Conditions != null)
                {
                    // This entry has conditions
                    AdvShowIfButton.Enabled = true;
                    AdvEditConditionsButton.Enabled = true;
                    AdvShowIfButton.Checked = true;
                    AdvConditions.Enabled = true;
                    AdvConditions.Conditions = Conditions;
                }
            }
            return Refresh(AdvEntryTextLabel, AdvEntryTextBox, AdvShowIfButton, AdvConditions, AdvEditConditionsButton);
        }

        public List<BaseWidget> UpdateTabView(dynamic Utility)
        {
            SetSimpleVisible(IsSimplifyAble());
            return Refresh(SimpleChoiceLabels, SimpleTooAdvancedLabel, SimpleChoiceTextBoxes, SimpleChoiceButtons,
                SimpleCancelLabel, SimpleDisallowButton, SimpleBranchButton, SimpleTooAdvancedLabel);
        }

        public List<BaseWidget> Sync(dynamic Utility)
        {
            List<string> choices = new List<string>();
            for (int i = 0; i < this.Text.Count; i++)
            {
                if (!string.IsNullOrEmpty(this.Text[i])) choices.Add(this.Text[i]);
            }
            this.Text = new List<string>(choices);
            if (this.Text.Count < 8) this.Text.AddRange(new string[8 - this.Text.Count]);
            if (ChoiceCancel != -1 && ChoiceCancel >= choices.Count) ChoiceCancel = choices.Count - 1;
            AdvList.Items = choices;
            if (AdvList.Index >= AdvList.Items.Count) AdvList.Index = AdvList.Items.Count - 1;
            if (AdvList.Index == -1 && choices.Count > 0) AdvList.Index = 0;
            AdvTreatAsChoiceBox.Items = choices;
            SimpleChoiceButtons.ForEach(c => c.Enabled = false);
            SimpleChoiceTextBoxes.ForEach(t => t.Text = "");
            for (int i = 0; i < choices.Count; i++)
            {
                SimpleChoiceTextBoxes[i].Text = choices[i];
                SimpleChoiceButtons[i].Enabled = true;
            }
            if (choices.Count == 0 && !DisallowCancel && !BranchCancel)
            {
                DisallowCancel = true;
                ChoiceCancel = -1;
            }
            SimpleDisallowButton.Selected = false;
            SimpleBranchButton.Selected = false;
            SimpleChoiceButtons.ForEach(c => c.Selected = false);
            AdvDisallowButton.Selected = false;
            AdvBranchButton.Selected = false;
            AdvTreatAsChoiceButton.Selected = false;
            AdvTreatAsChoiceButton.Enabled = choices.Count > 0;
            AdvTreatAsChoiceBox.Enabled = false;
            if (DisallowCancel)
            {
                SimpleDisallowButton.Selected = true;
                AdvDisallowButton.Selected = true;
            }
            else if (BranchCancel)
            {
                SimpleBranchButton.Selected = true;
                AdvBranchButton.Selected = true;
            }
            else if (ChoiceCancel != -1)
            {
                AdvTreatAsChoiceButton.Selected = true;
                AdvTreatAsChoiceBox.Enabled = true;
                AdvTreatAsChoiceBox.Index = ChoiceCancel;
                SimpleChoiceButtons[ChoiceCancel].Selected = true;
            }
            AdvEntryTextBox.Text = AdvList.Index >= 0 ? AdvList.Items[AdvList.Index] : "";
            return Refresh(AdvTreatAsChoiceBox, AdvTreatAsChoiceButton, AdvDisallowButton,
                AdvEntryTextBox, AdvBranchButton, SimpleChoiceTextBoxes, SimpleChoiceButtons,
                SimpleDisallowButton, SimpleBranchButton, AdvList);
        }

        public void SetSimpleVisible(bool Visible)
        {
            SimpleChoiceLabels.ForEach(w => w.Visible = Visible);
            SimpleChoiceTextBoxes.ForEach(w => w.Visible = Visible);
            SimpleChoiceButtons.ForEach(w => w.Visible = Visible);
            SimpleCancelLabel.Visible = Visible;
            SimpleDisallowButton.Visible = Visible;
            SimpleBranchButton.Visible = Visible;
            SimpleTooAdvancedLabel.Visible = !Visible;
        }

        public bool IsSimplifyAble()
        {
            if (AdvStoreInVariableButton.Checked) return false;
            if (this.Conditions.Find(c => c != null && c.Count > 0) != null) return false;
            return true;
        }

        public void SaveWindow(dynamic Utility)
        {
            int choices = 0;
            for (int i = 0; i < 8; i++)
            {
                TextBox textbox = SimpleChoiceTextBoxes[i];
                if (!string.IsNullOrEmpty(textbox.Text) || i == 0) choices++;
            }
            if (Utility.TooManyBranches(choices))
            {
                // Lets the edit window not to close yet; await the point when CanClose is set to true.
                Utility.AwaitCloseFlag = true;
                Utility.CanClose = false;
                // Shows a warning window that you're about to delete non-empty branch(es).
                Utility.ShowWindow(
                    "Warning",
                    "You are about to delete one or more non-empty branches and their commands. Is this OK?",
                    IconType.Warning,
                    ButtonType.YesNo,
                    (ChoiceCallback) delegate (int ChoiceIndex)
                    {
                        if (ChoiceIndex == 0)
                        {
                            Utility.CanClose = true;
                            Save(Utility);
                        }
                    });
            }
            else Save(Utility);
        }

        public void Save(dynamic Utility)
        {
            Utility.RedrawAll = true;
            List<object> Choices = new List<object>();
            object Cancel = null;
            for (int i = 0; i < 8; i++)
            {
                RadioButton radiobutton = SimpleChoiceButtons[i];
                TextBox textbox = SimpleChoiceTextBoxes[i];
                if (!string.IsNullOrEmpty(textbox.Text) || i == 0)
                {
                    Choices.Add(textbox.Text?? "");
                }
                if (radiobutton.Selected)
                {
                    Cancel = i;
                }
            }
            if (SimpleDisallowButton.Selected) Cancel = ":disallow";
            else if (SimpleBranchButton.Selected) Cancel = ":branch";
            Utility.SetParam("choices", Choices);
            Utility.SetParam("cancel", Cancel);

            // Deletes any branches that are too many (e.g. deletes the 8th branch if it had 8, and the new number of branches (Choices.Count) is 7).
            // Or creates as many new branches and commands as are missing (e.g. creates one command if it went from 3 to 4 branches).
            // Calls the delegate to create the new branch command.
            Utility.UpdateBranchCommands(Choices.Count, (BranchCallback) delegate (int BranchIndex, int CommandIndent)
            {
                return Utility.CreateCommand(CommandIndent, "choice_branch", new Dictionary<string, object>() { { ":choice_index", BranchIndex } });
            });
        }

        public void CreateBlank(dynamic Utility)
        {
            Utility.CreateParam("choices", new List<object>());
            Utility.CreateParam("cancel", ":disallow");
        }
    }
}
