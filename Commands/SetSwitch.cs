using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI.Commands
{
    public class SetSwitch : Command
    {
        protected MultilineLabel SwitchLabel;
        protected Label MakeLabel;
        protected SwitchPicker MainSwitchPicker;
        protected Dropdown Operator;
        protected RadioButton ConstantButton;
        protected Dropdown ConstantBox;
        protected RadioButton SwitchButton;
        protected SwitchPicker SwitchBox;
        protected RadioButton ScriptButton;
        protected TextBox ScriptBox;

        public SetSwitch() : base("set_switch", "Set Game Switch")
        {
            this.OnCreateReadOnly.Add(CreateReadOnly);
            this.OnLoadReadOnly.Add(LoadReadOnly);
            this.OnCreateWindow.Add(CreateWindow);
            this.OnSaveWindow.Add(SaveWindow);
            this.OnCreateBlank.Add(CreateBlank);
            this.HeaderColor = HeaderColors.GREEN;
            this.TextColors.Add(new Color(0, 255, 0)); // Color 1
            this.TextColors.Add(new Color(255, 0, 0)); // Color 2
            this.TextColors.Add(new Color(255, 0, 255)); // Color 3
            this.WindowWidth = 370;
            this.WindowHeight = 200;
            this.PickerTabName = "General";
        }

        public List<BaseWidget> CreateReadOnly()
        {
            SwitchLabel = new MultilineLabel("switchlabel");
            SwitchLabel.Parse = true;
            return Refresh(SwitchLabel);
        }

        public List<BaseWidget> LoadReadOnly(dynamic Utility)
        {
            // Make Game Switch [001: Talked to Rival] equal to ON
            // Make Game Switch [001: Talked to Rival] opposite of Time.now.day == 1
            // Make Game Switch [001: Talked to Rival] equal to Game Switch [002: Obtained Starter]
            string switchid = Utility.Digits("switch_id", 3);
            string switchname = Utility.GetSwitchName("group_id", "switch_id");
            string txt = $"Make Game Switch [{switchid}: {switchname}] ";
            if (Utility.ParamAsString("operator") == ":equal") txt += "equal to ";
            else txt += "opposite of ";
            if (Utility.ParamIsBool("value"))
            {
                txt += Utility.ParamAsBool("value") ? "[c=1]ON" : "[c=2]OFF";
            }
            else if (Utility.ParamIsString("value"))
            {
                txt += $"[c=3]{Utility.ParamAsString("value")}";
            }
            else if (Utility.ParamIsHash("value"))
            {
                Dictionary<string, object> param = Utility.ParamAsHash("value");
                txt += $"Game Switch [{Utility.Digits((int) param[":switch_id"], 3)}: {Utility.GetSwitchName((int) param[":group_id"], (int) param[":switch_id"])}]";
            }
            SwitchLabel.Text = txt;
            return Refresh(SwitchLabel);
        }

        public List<BaseWidget> CreateWindow(dynamic Utility)
        {
            MakeLabel = new Label("make");
            MakeLabel.Text = "Make Switch";
            MakeLabel.X = 9;
            MakeLabel.Y = 32;
            MainSwitchPicker = new SwitchPicker("mainswitch");
            MainSwitchPicker.X = 87;
            MainSwitchPicker.Y = 26;
            MainSwitchPicker.Width = 160;
            MainSwitchPicker.Height = 25;
            MainSwitchPicker.GroupID = Utility.ParamAsInt("group_id");
            MainSwitchPicker.SwitchID = Utility.ParamAsInt("switch_id");
            Operator = new Dropdown("operator");
            Operator.X = 253;
            Operator.Y = 26;
            Operator.Width = 100;
            Operator.Height = 25;
            Operator.Items = new List<string>() { "Equal to", "Opposite of" };
            Operator.Index = Utility.ParamAsString("operator") == ":equal" ? 0 : 1;
            ConstantButton = new RadioButton("constantlabel");
            ConstantButton.X = 11;
            ConstantButton.Y = 64;
            ConstantButton.Text = "Constant:";
            ConstantButton.OnSelected += ButtonPressed;
            ConstantBox = new Dropdown("constantbox");
            ConstantBox.X = 91;
            ConstantBox.Y = 61;
            ConstantBox.Items = new List<string>() { "Enabled", "Disabled" };
            ConstantBox.Width = 90;
            ConstantBox.Height = 25;
            SwitchButton = new RadioButton("switchlabel");
            SwitchButton.X = 11;
            SwitchButton.Y = 94;
            SwitchButton.OnSelected += ButtonPressed;
            SwitchBox = new SwitchPicker("switchbox");
            SwitchBox.X = 91;
            SwitchBox.Y = 91;
            SwitchBox.Width = 165;
            SwitchBox.Height = 25;
            SwitchButton.Text = "Switch:";
            ScriptButton = new RadioButton("scriptlabel");
            ScriptButton.X = 11;
            ScriptButton.Y = 124;
            ScriptButton.Text = "Script:";
            ScriptButton.OnSelected += ButtonPressed;
            ScriptBox = new TextBox("scriptbox");
            ScriptBox.X = 91;
            ScriptBox.Y = 121;
            ScriptBox.Width = 165;
            ScriptBox.Height = 27;
            UpdateSelection(Utility);
            return Refresh(MakeLabel, MainSwitchPicker, Operator, ConstantButton, ConstantBox, SwitchButton, SwitchBox, ScriptButton, ScriptBox);
        }
        
        public List<BaseWidget> ButtonPressed()
        {
            ConstantBox.Enabled = SwitchBox.Enabled = ScriptBox.Enabled = false;
            if (ConstantButton.Selected)
            {
                ConstantBox.Enabled = true;
                ConstantBox.Index = 0;
            }
            else if (SwitchButton.Selected)
            {
                SwitchBox.Enabled = true;
                SwitchBox.GroupID = 1;
                SwitchBox.SwitchID = 1;
            }
            else if (ScriptButton.Selected)
            {
                ScriptBox.Enabled = true;
                ScriptBox.Text = "";
            }
            return Refresh(ConstantBox, SwitchBox, ScriptBox);
        }

        public void UpdateSelection(dynamic Utility)
        {
            ConstantButton.Selected = SwitchButton.Selected = ScriptButton.Selected = false;
            ConstantBox.Enabled = SwitchBox.Enabled = ScriptBox.Enabled = false;
            if (Utility.ParamIsBool("value"))
            {
                ConstantButton.Selected = true;
                ConstantBox.Enabled = true;
                ConstantBox.Index = Utility.ParamAsBool("value") ? 0 : 1;
            }
            else if (Utility.ParamIsHash("value"))
            {
                SwitchButton.Selected = true;
                SwitchBox.Enabled = true;
                SwitchBox.GroupID = (int) Utility.ParamAsHash("value")[":group_id"];
                SwitchBox.SwitchID = (int) Utility.ParamAsHash("value")[":switch_id"];
            }
            else if (Utility.ParamIsString("value"))
            {
                ScriptButton.Selected = true;
                ScriptBox.Enabled = true;
                ScriptBox.Text = Utility.ParamAsString("value");
            }
        }

        public void SaveWindow(dynamic Utility)
        {
            Utility.SetParam("group_id", MainSwitchPicker.GroupID);
            Utility.SetParam("switch_id", MainSwitchPicker.SwitchID);
            Utility.SetParam("operator", Operator.Index == 0 ? ":equal" : ":opposite");
            if (ConstantButton.Selected) Utility.SetParam("value", ConstantBox.Index == 0);
            else if (SwitchButton.Selected) Utility.SetParam("value", new Dictionary<string, object>() { { ":group_id", SwitchBox.GroupID }, { ":switch_id", SwitchBox.SwitchID } });
            else if (ScriptButton.Selected) Utility.SetParam("value", ScriptBox.Text);
        }

        public void CreateBlank(dynamic Utility)
        {
            Utility.CreateParam("group_id", 1);
            Utility.CreateParam("switch_id", 1);
            Utility.CreateParam("operator", ":equal");
            Utility.CreateParam("value", true);
        }
    }
}
