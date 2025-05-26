using System;
using Godot;

public partial class PeripheralDisplay : Button {
    [Export] private Label icon;
    private Action onClick;


    public void Setup(Action onClick, string iconGlyph, string name, bool highlight) {
        ThemeTypeVariation = highlight ? "buttonHighlight" : "buttonDefault";
        Text = name;
        icon.Text = iconGlyph;
    }

    public override void _Pressed() {
        onClick();
        base._Pressed();
    }
}
