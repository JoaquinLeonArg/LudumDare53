using Godot;

public class ChatListItem: Control {
    private ColorRect area;
    private bool hovering;
    public int chatIndex;
    public bool active;
    public override void _Ready() {
        this.area = GetNode<ColorRect>("Area");
        this.area.Connect("mouse_entered", this, nameof(HandleMouseEntered));
        this.area.Connect("mouse_exited", this, nameof(HandleMouseExited));
        this.area.Connect("gui_input", this, nameof(HandleClick));
    }
    public override void _Process(float delta) {
        if (this.active) {
            this.area.Color = new Color("ff4aa7");
        } else if (this.hovering) {
            this.area.Color = new Color("ff8370");
        } else {
            this.area.Color = new Color("482254");
        }
    }
    private void HandleMouseEntered() {
        this.hovering = true;
    }
    private void HandleMouseExited() {
        this.hovering = false;
    }
    private void HandleClick(InputEvent inputEvent) {
        if (inputEvent == null) { return; }
        if (inputEvent is InputEventMouseButton buttonEvent) {
            if (buttonEvent.IsActionPressed("mouse_left") && this.hovering) {
                GameManager.chatWindow.SetActiveChat(this.chatIndex);
            }
        }
    }
    public void SetChatName(string name) {
        this.GetNode<RichTextLabel>("Name").Text = name;
    }
}
