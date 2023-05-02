using Godot;

public partial class BaseWindow: Control {
    private const float MIN_MARGIN = 20;
    protected bool dragging = false;
    protected Vector2 dragOffset = Vector2.Zero;
    protected Panel dragArea = null;
    public override void _Ready() {
        this.dragArea = GetNode<Panel>("DragArea");
        if (this.dragArea == null) {
            throw new System.Exception("Cannot find DragArea for node");
        }
    }
    public void HandleInputEvent(InputEvent inputEvent) {
        if (inputEvent == null) { return; }
        if (inputEvent is InputEventMouseButton buttonEvent) {
            if (buttonEvent.IsActionPressed("mouse_left") && !dragging) {
                this.dragging = true;
                this.dragOffset = GetGlobalMousePosition() - this.GetGlobalRect().Position;
                GetParent().MoveChild(this, GetParent().GetChildCount());

            }
            if (buttonEvent.IsActionReleased("mouse_left") && dragging) {
                this.dragging = false;
                float newPositionX = (float)Mathf.Clamp(
                    this.RectGlobalPosition.x,
                    BaseWindow.MIN_MARGIN,
                    GetViewportRect().Size.x - this.GetRect().Size.x - BaseWindow.MIN_MARGIN
                );
                float newPositionY = (float)Mathf.Clamp(
                    this.RectGlobalPosition.y,
                    0,
                    GetViewportRect().Size.y - BaseWindow.MIN_MARGIN * 12
                );
                this.SetGlobalPosition(new Vector2(newPositionX, newPositionY));
                this.dragOffset = Vector2.Zero;
            }
        }
        if (inputEvent is InputEventMouseMotion _) {
            if (this.dragging) {
                this.RectGlobalPosition = GetGlobalMousePosition() - this.dragOffset;
            }
        }
    }
    public virtual void Open(Vector2 from) {
        this.Visible = true;
        var to = this.RectGlobalPosition;
        this.RectGlobalPosition = from;
        this.RectScale = Vector2.Zero;
        var tween = CreateTween();
        tween.SetParallel(true);
        tween.TweenProperty(this, "rect_scale", new Vector2(1.0f, 1.0f), 0.2f).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(this, "rect_global_position", to, 0.2f).SetTrans(Tween.TransitionType.Quad);
        GetParent().MoveChild(this, GetParent().GetChildCount());
    }
    public void Close() {
        var tween = CreateTween();
        tween.TweenProperty(this, "rect_scale", new Vector2(0.0f, 0.0f), 0.2f).SetTrans(Tween.TransitionType.Quad);
        tween.TweenProperty(this, "visible", false, 0.0f);
    }
}
