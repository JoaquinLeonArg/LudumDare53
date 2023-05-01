using Godot;
using System.Text.RegularExpressions;

public class LoginWindow: BaseWindow {
	private Control buttonNode;
	private LineEdit usernameNode;
	private LineEdit passwordNode;
	private RichTextLabel errorNode;
	private bool hovering;
	public bool done;
	public override void _Ready() {
		this.buttonNode = GetNode<Control>("Login");
		this.usernameNode = GetNode<LineEdit>("Username");
		this.passwordNode = GetNode<LineEdit>("Password");
		this.errorNode = GetNode<RichTextLabel>("InvalidCreds");
		this.buttonNode.Connect("mouse_entered", this, nameof(HandleMouseEntered));
		this.buttonNode.Connect("mouse_exited", this, nameof(HandleMouseExited));
		this.buttonNode.Connect("gui_input", this, nameof(HandleClick));
	}
	public override void _Process(float delta) {
		if (this.hovering) {
			this.buttonNode.Modulate = new Color("ff9999");
		} else {
			this.buttonNode.Modulate = new Color("ffffff");
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
			if (buttonEvent.IsActionPressed("mouse_left") && this.hovering && !this.done) {
				if (usernameNode.Text.Length < 2) {
					this.errorNode.BbcodeText = "[center]Username is too short.[/center]";
					return;
				}
				if (!Regex.IsMatch(usernameNode.Text, "^[a-zA-Z0-9]+$")) {
					this.errorNode.BbcodeText = "[center]Username contains invalid characters.[/center]";
					return;
				}
				if (passwordNode.Text != "password123") {
					this.errorNode.BbcodeText = "[center]Wrong password.[/center]";
					return;
				}
				this.done = true;
				GameManager.playerName = usernameNode.Text;
				this.Close();
				var tween = CreateTween();
				tween.SetParallel(false);
				tween.TweenProperty(GetTree().CurrentScene.GetNode("StickyNote"), "modulate:a", 0.0f, 0.5f);
				tween.TweenCallback(GetTree().CurrentScene.GetNode("StickyNote"), "queue_free");
				tween.TweenProperty(this.GetParent(), "modulate:a", 0.0f, 0.5f);
				tween.TweenCallback(this.GetParent(), "queue_free");
				GameManager.timeline.Start();
			}
		}
	}
}
