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
        public override void _Ready()
        {
            lineEdit.Text = "";
            base._Ready();
        }
        public void SetupStr(string initValue, Action<string> output, bool editable) {
            lineEdit.Text = initValue.ToString();
            lineEdit.Editable = editable;
            lineEdit.TextChanged += (t) =>
            {
                output(t);
            };
        }

        public void Setup(float initValue, Action<float> output, bool editable) {
            lineEdit.Text = initValue.ToString();
            lineEdit.Editable = editable;
            lineEdit.TextChanged += (t) =>
            {
                ParseAndClamp(t, output);
            };
        }

        public void ParseAndClamp(string text, Action<float> output) {
            var value = text.ToFloat();
            value = (min == 0 && max == 0) ? value : Mathf.Clamp(value, min, max);
            output(value);
        }
    }
}
