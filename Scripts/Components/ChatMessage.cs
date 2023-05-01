using Godot;
using System.Collections.Generic;

public enum ChatMessageSide {
	Left,
	Right
}

public enum ChatMessageOptions {
	Yelling
}

public class ChatMessage: BaseChatMessage {
	private RichTextLabel leftName;
	private RichTextLabel rightName;
	private RichTextLabel message;
	private ColorRect background;
	public override void _Ready() {
		this.leftName = GetNode<RichTextLabel>("NameLeft");
		this.rightName = GetNode<RichTextLabel>("NameRight");
		this.message = GetNode<RichTextLabel>("Message");
		this.background = GetNode<ColorRect>("Background");
	}

	public override void _Process(float delta) {
		this.UpdateBackground();
	}

	public void SetData(ChatMessageSide side, string name, string message, Dictionary<ChatMessageOptions, int> opts) {
		if (side == ChatMessageSide.Left) {
			this.rightName.Visible = false;
			this.leftName.BbcodeText = name;
		} else if (side == ChatMessageSide.Right) {
			this.leftName.Visible = false;
			this.rightName.BbcodeText = "[right]You[/right]";
		}
		this.message.BbcodeText = message;
		CallDeferred("UpdateBackground");
	}
	public void UpdateBackground() {
		// Workaround to let the text label to update it's size.
		this.background.SetSize(new Vector2(this.background.RectSize.x, this.message.RectSize.y / 10 + 8));
	}

	public override int GetHeight() {
		return (int)this.message.RectSize.y / 10 + 64;
	}
}
