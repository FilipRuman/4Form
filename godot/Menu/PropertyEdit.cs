namespace ForForm.Menu
{
    using System;
    using Godot;

    public partial class PropertyEdit : Control {
        [Export]
        LineEdit lineEdit;

        // [Export]
        // not used anywhere currently
        float min = 0,
            max = 0;
        bool alreadyDoneTextSetup = false;

        public override void _Ready() {
            if (!alreadyDoneTextSetup)
                lineEdit.Text = "";
            base._Ready();
        }

        public void SetupStr(string initValue, Action<string> output, bool editable) {
            alreadyDoneTextSetup = true;
            lineEdit.Text = initValue;
            lineEdit.Editable = editable;
            lineEdit.TextChanged += (t) =>
            {
                output(t);
            };
        }

        public void Setup(float initValue, Action<float> output, bool editable) {
            alreadyDoneTextSetup = true;
            lineEdit.Text = initValue.ToString();
            lineEdit.Editable = editable;
            lineEdit.TextChanged += (t) =>
            {
                ParseAndClamp(t, output);
            };
        }

        public void ParseAndClamp(string text, Action<float> output) {
            if (
                !float.TryParse(
                    text,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out float value
                )
            )
                return;
            value = (min == 0 && max == 0) ? value : Mathf.Clamp(value, min, max);
            output(value);
        }
    }
}
