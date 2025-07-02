using Godot;

namespace ForForm.Menu.Animations
{
    public interface IUIAnimation {
        public void Run(Control _target);
        public void Process(float delta);
    }
}
