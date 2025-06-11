namespace ForForm.Menu
{
    using Godot;

    public partial class MenuTabContent :  Control {

        [ExportGroup("lock screen settings")]
        [Export]
        public bool gameModeMustBeSelected;

        [Export]
        public bool gameCantBeStarted;
    }
}
