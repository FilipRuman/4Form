namespace ForForm.Progression
{
    using Godot;

    public partial class Wearable : Resource {
        [Export]
        public Type type;

        [Export]
        public Image icon;

        [Export]
        public string name;

        [Export]
        public uint price;

        [Export]
        public float airDrag,
            mass_kg,
            durability;

        public float wear; // 0-1

        public enum Type {
            shoes,
            helmet,
            socks,
            pants,
            jersey,
        }
    }
}
