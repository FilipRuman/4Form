using System;
using Godot;

public partial class PeripheralDisplay : Button {

    [Export] public Label icon;
    public Action onClick;


    public void Setup(Action onClick, string iconGlyph, string name, bool highlight) {
        ThemeTypeVariation = highlight ? "buttonHighlight" : "buttonDefault";
        Text = name;
        icon.Text = iconGlyph;
    }
public void Highlight(){

        ThemeTypeVariation =  "buttonHighlight";
}
    public override void _Pressed() {
        onClick();
        base._Pressed();
    }
}
