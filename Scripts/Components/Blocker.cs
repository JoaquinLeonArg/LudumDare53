using Godot;

public class Blocker: Panel {
    public override void _Ready() {
        Connect("gui_input", this, nameof(HandleInputEvent));
    }
    private void HandleInputEvent(InputEvent inputEvent) { }
}
