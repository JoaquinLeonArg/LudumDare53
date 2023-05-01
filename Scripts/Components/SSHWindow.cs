using Godot;

public class SSHWindow: BaseWindow {
	private LineEdit textEntryNode;
	private RichTextLabel messagesNode;
	public override void _Ready() {
		GameManager.sshWindow = this;
		this.textEntryNode = GetNode<LineEdit>("LineEdit");
		this.messagesNode = GetNode<RichTextLabel>("ChatMessages");
		this.textEntryNode.Connect("text_entered", this, nameof(OnTextSent));
	}
	private void OnTextSent(string text) {
		GameManager.DroneCommand(text);
		this.textEntryNode.Text = "";
	}
	public void AddText(string text) {
		this.messagesNode.BbcodeText += text;
		CallDeferred(nameof(AdjustPosition));
	}

	private void AdjustPosition() {
		this.messagesNode.SetPosition(new Vector2(this.messagesNode.RectPosition.x, 580 - (this.messagesNode.RectSize.y / 10)));
	}
}
