namespace ForForm.Menu.Animations
{
    using Godot;

    [GlobalClass, Tool,Icon("res://Script icons/air.png")]
    public partial class SwipeAnimation : Resource, IUIAnimation {
        [Export]
        Vector2 targetPosition,
            startPosition;

        [Export]
        bool hideAtTheEnd;

        [Export]
        Curve animationCurve; // but not the one from unity
        Control target;
        bool running;
        float offset;

        public void Process(float delta) {
            if (!running)
                return;
            offset += delta;

            float animationPercent = animationCurve.SampleBaked(offset);

            target.Position = startPosition.Lerp(targetPosition, animationPercent);

            if (animationPercent >= 1) {
                running = false;
                if (hideAtTheEnd)
                    target.Visible = false;
            }
        }

        public void Run(Control _target) {
            target = _target;

            target.Visible = true;
            offset = 0;
            running = true;
        }
    }
}
