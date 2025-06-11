namespace ForForm.Menu
{
    using Godot;

    public static class UIMiscs {
        public static void ClearChildren(Node node) {
            foreach (var child in node.GetChildren()) {
                child.QueueFree();
            }
        }
    }
}
