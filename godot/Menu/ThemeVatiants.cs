namespace ForForm.Menu
{
    using Godot;

    public static class ThemeVariants {
        public const string buttonDefault = "buttonDefault";
        public const string buttonHighlight = "buttonHighlight";

        public static void SetForButton(bool highlight, Button button) =>
            button.ThemeTypeVariation = highlight ? buttonHighlight : buttonDefault;
    }
}
