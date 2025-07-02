namespace ForForm.Menu.Animations
{
    using Godot;

    [Tool, Icon("res://Script icons/animation.png")]
    public partial class UIAnimationPlayer : Node {
        [Export]
        Resource animation;
        [Export]
        Resource reverseAnimation;


        [Export]
        Control target;

        [ExportToolButton("Run")]
        Callable RunButton => Callable.From(Run);

        [ExportToolButton("Run in reverse")]
        Callable RunInReverseButton => Callable.From(RunInReverse);

        IUIAnimation animationInterface;

        public override void _Process(double delta) {
            if (animationInterface != null)
                animationInterface.Process(((float)delta));

            base._Process(delta);
        }

        public void Run() {
            animationInterface = animation as IUIAnimation;
            if (animationInterface != null)
                animationInterface.Run(target);
        }

        public void RunInReverse() {
            animationInterface = reverseAnimation as IUIAnimation;
            if (animationInterface != null)
                animationInterface.Run(target);
        }
    }
}
