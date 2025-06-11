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

        public void Setup(float initValue, Action<float> output) {
            lineEdit.Text = initValue.ToString();
            lineEdit.Editable = GameConfig.GameSettings.CurrentGameMode.canEditBikeModels;
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
