using Godot;

public class ViewerWindow: BaseWindow {
	private Control leftArrowNode;
	private Control rightArrowNode;
	private Sprite contentNode;
	private RichTextLabel pageCountNode;
	public override void _Ready() {
		GameManager.viewerWindow = this;
		this.leftArrowNode = GetNode<Control>("LeftArrow");
		this.rightArrowNode = GetNode<Control>("RightArrow");
		this.contentNode = GetNode<Sprite>("Content");
		this.pageCountNode = GetNode<RichTextLabel>("PageNumber");
		this.SetContent(ResourceLoader.Load<Texture>("res://Resources/Image/example_pdf.png"));
		this.UpdatePagination();
		this.leftArrowNode.Connect("gui_input", this, nameof(HandleLeftArrow));
		this.rightArrowNode.Connect("gui_input", this, nameof(HandleRightArrow));
	}

	public void SetContent(Texture content) {
		var contentPages = (int)content.GetWidth() / 580;
		this.contentNode.Texture = content;
		this.contentNode.Hframes = contentPages;
		this.contentNode.Frame = 0;
	}

	private void HandleLeftArrow(InputEvent inputEvent) {
		if (inputEvent == null) { return; }
		if (inputEvent is InputEventMouseButton buttonEvent) {
			if (buttonEvent.IsActionPressed("mouse_left") && this.contentNode.Frame > 0) {
				this.contentNode.Frame--;
				UpdatePagination();
			}
		}
	}
	private void HandleRightArrow(InputEvent inputEvent) {
		if (inputEvent == null) { return; }
		if (inputEvent is InputEventMouseButton buttonEvent) {
			if (buttonEvent.IsActionPressed("mouse_left") && this.contentNode.Frame < this.contentNode.Hframes - 1) {
				this.contentNode.Frame++;
				UpdatePagination();
			}
		}
	}
	private void UpdatePagination() {
		if (this.contentNode.Frame == 0) { this.leftArrowNode.Visible = false; } else { this.leftArrowNode.Visible = true; }
		if (this.contentNode.Frame == this.contentNode.Hframes - 1) { this.rightArrowNode.Visible = false; } else { this.rightArrowNode.Visible = true; }
		this.pageCountNode.BbcodeText = "[center]" + (this.contentNode.Frame + 1).ToString().PadRight(3) + "/" + this.contentNode.Hframes.ToString().PadLeft(3);
	}
}
