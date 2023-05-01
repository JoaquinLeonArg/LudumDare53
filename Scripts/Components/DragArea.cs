using Godot;

public partial class DragArea: Panel {
	private BaseWindow parent;
	public override void _Ready() {
		this.parent = this.GetParent<BaseWindow>();
		Connect("gui_input", this.parent, "HandleInputEvent");
	}
}
