using Godot;
using System.Collections.Generic;

public class ChatMessageList: Control {
    private readonly PackedScene chatMessageScene = GD.Load<PackedScene>("res://Components/Sluck/ChatMessage.tscn");
    private readonly PackedScene chatGifScene = GD.Load<PackedScene>("res://Components/Sluck/ChatGif.tscn");

    private int lastResponse;
    public void AddChatMessage(ChatMessageSide side, string name, string message, Dictionary<ChatMessageOptions, int> opts) {
        var chatMessage = chatMessageScene.Instance<ChatMessage>();
        AddChild(chatMessage);
        chatMessage.SetData(side, name, message, opts);
        if (side == ChatMessageSide.Right) {
            chatMessage.GetNode<ColorRect>("Background").Color = new Color("#fc92fa");
            chatMessage.GetNode<RichTextLabel>("Message").AddColorOverride("default_color", new Color("#482254"));
        }
    }
    public void AddGifMessage(ChatMessageSide side, string name, Texture texture) {
        var chatMessage = chatGifScene.Instance<ChatGif>();
        AddChild(chatMessage);
        chatMessage.SetData(side, name, texture);
    }
    override public void _Process(float delta) {
        if (GetChildCount() == 0) return;
        var sumH = 0;
        for (var i = 0; i < GetChildCount(); i++) { // Iterate from last to first
            var child = GetChild<BaseChatMessage>(GetChildCount() - i - 1);
            if (i > 10) { child.QueueFree(); } else {
                sumH += child.GetHeight();
                child.SetPosition(new Vector2(0, 540 - sumH));
            }
        }
    }
    public void SetLastResponse(int responseNumber) {
        this.lastResponse = responseNumber;
    }
    public int GetAndResetLastResponse() {
        var response = this.lastResponse;
        this.lastResponse = 0;
        return response;
    }
}
