using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class Message : Command
    {
        protected Label TextLabel;
        protected MultilineTextBox TextBox;

        public Message() : base("message", "Show Text")
        {
            this.OnCreateReadOnly.Add(CreateReadOnly);
            this.OnLoadReadOnly.Add(LoadReadOnly);
            this.OnCreateWindow.Add(CreateWindow);
            this.OnSaveWindow.Add(SaveWindow);
            this.HeaderColor = HeaderColors.GREEN;
            this.WindowWidth = 400;
            this.WindowHeight = 200;
        }

        public List<BaseWidget> CreateReadOnly()
        {
            TextLabel = new MultilineLabel("label");
            return Refresh(TextLabel);
        }

        public List<BaseWidget> LoadReadOnly(dynamic Utility)
        {
            TextLabel.Text = Utility.ParamAsString("text");
            return Refresh(TextLabel);
        }

        public List<BaseWidget> CreateWindow(dynamic Utility)
        {
            TextBox = new MultilineTextBox("textbox");
            TextBox.X = 4;
            TextBox.Y = 26;
            TextBox.Width = 392;
            TextBox.Height = 132;
            TextBox.Text = Utility.ParamAsString("text");
            TextBox.Index = TextBox.Text.Length;
            TextBox.Focus = true;
            return Refresh(TextBox);
        }

        public void SaveWindow(dynamic Utility)
        {
            Utility.SetParam("text", TextBox.Text);
        }
    }
}
