namespace ForForm.Menu
{
    using System;
    using Godot;

    public partial class SimpleSelectionUI: Button {
        [Export]
        TextureRect textureRect;

        [Export]
        Label nameLabel;

        public void Setup(string name, Texture2D icon, Action _OnClick) {
            Pressed += _OnClick;
            nameLabel.Text = name;
            textureRect.Texture = icon;
        }
    }
}
