namespace ForForm.Menu.BikeConfig
{
    using System;
    using Godot;

    public partial class BikeModelSelectionUI : Button {
        [Export]
        TextureRect textureRect;

        [Export]
        Label nameLabel;

        public void Setup(Bike.BikeModel bikeModel, Action _OnClick) {
            Pressed += _OnClick;
            nameLabel.Text = bikeModel.name;
            textureRect.Texture = bikeModel.icon;
        }
    }
}
