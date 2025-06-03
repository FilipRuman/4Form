namespace ForForm.Menu
{
    using System;
    using Godot;

    public partial class PeripheralDisplay : Button {
        [Export]
        public Label icon;
        public Action onClick;

        public void Setup(Action _onClick, string iconGlyph, string name, bool highlight) {
            Highlight(highlight);
            Text = name;
            icon.Text = iconGlyph;
            onClick = _onClick;
        }

        public void Highlight(bool highlight) {
            ThemeTypeVariation = highlight ? "buttonHighlight" : "buttonDefault";
        }

        public override void _Pressed() {
            onClick();
            base._Pressed();
        }
    }
}
