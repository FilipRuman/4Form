namespace ForForm.Menu
{
    using System;
    using Godot;

    public partial class PropertyEdit : Control {
        [Export]
        LineEdit lineEdit;

        [Export]
        float min,
            max;

        public void Setup(float initValue,Action<float> output,bool editable = true) {
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

            // lineEdit.Text = value.ToString();
        }
    }
}
