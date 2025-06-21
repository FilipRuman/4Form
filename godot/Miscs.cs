namespace ForForm
{
    using Godot;

    public static class Miscs {
        public static void ClearChildren(Node node) {
            foreach (var child in node.GetChildren()) {
                child.QueueFree();
            }
        }
    }
}
