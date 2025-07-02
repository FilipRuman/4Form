namespace ForForm.Menu
{
    using Godot;

    public partial class MenuTabContent : Control {
        [Export]
        public Animations.UIAnimationPlayer animationPlayer;

        [ExportGroup("lock screen settings")]
        [Export]
        public bool gameCantBeStarted;
    }
}
