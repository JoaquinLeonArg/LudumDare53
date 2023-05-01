using Godot;

public partial class CloseArea: Panel {
    private BaseWindow parent;
    public override void _Ready() {
        this.parent = this.GetParent<BaseWindow>();
        Connect("gui_input", this, nameof(HandleInputEvent));
    }
    private void HandleInputEvent(InputEvent inputEvent) {
        if (inputEvent == null) { return; }
        if (inputEvent is InputEventMouseButton buttonEvent) {
            if (buttonEvent.IsActionPressed("mouse_left")) {
                //this.parent.Visible = false;
                this.parent.Close();
            }
        }
    }
}
