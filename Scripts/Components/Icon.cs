using Godot;

public class Icon: Control {
    [Export]
    private string iconName;
    [Export]
    private Texture iconImage;
    [Export]
    private NodePath window;

    private RichTextLabel nameNode;
    private Sprite spriteNode;
    private ColorRect selectedNode;
    private BaseWindow windowNode;
    private int doubleClickTimer;
    private bool selected;
    override public void _Ready() {
        this.nameNode = GetNode<RichTextLabel>("Name");
        this.spriteNode = GetNode<Sprite>("Icon");
        this.selectedNode = GetNode<ColorRect>("Selected");
        this.windowNode = GetNode<BaseWindow>(window);

        this.nameNode.BbcodeText = "[center]" + iconName + "[/center]";
        this.spriteNode.Texture = iconImage;
        Connect("gui_input", this, nameof(HandleGuiInput));
    }
    public override void _Process(float delta) {
        if (this.doubleClickTimer > 0) { this.doubleClickTimer -= 1; }
        if (this.selected) { this.selectedNode.Visible = true; } else { this.selectedNode.Visible = false; }
    }
    private void HandleGuiInput(InputEvent inputEvent) {
        if (inputEvent == null) { return; }
        if (inputEvent is InputEventMouseButton buttonEvent) {
            if (buttonEvent.IsActionPressed("mouse_left") && this.doubleClickTimer > 0) { // Double click
                this.selected = false;
                if (!this.windowNode.Visible) {
                    this.windowNode.Visible = true;
                    this.windowNode.Open(this.RectGlobalPosition);
                }
                this.doubleClickTimer = 0;
            }
            if (buttonEvent.IsActionPressed("mouse_left") && this.doubleClickTimer == 0) { // First click
                foreach (var genericIcon in GetParent().GetChildren()) {
                    var icon = genericIcon as Icon;
                    icon.selected = false;
                }
                this.selected = true;
                this.doubleClickTimer = 60; // 1 second to click again
            }
        }
    }
    public void ShowIcon() {
        this.Visible = true;
        this.RectScale = new Vector2(0, 0);
        var tween = CreateTween();
        tween.TweenProperty(this, "rect_scale", new Vector2(1, 1), 0.25f).SetTrans(Tween.TransitionType.Bounce);
    }
}
