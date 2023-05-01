using Godot;


public class ChatGif: BaseChatMessage {
    private RichTextLabel leftName;
    private RichTextLabel rightName;
    private Sprite gif;

    private int t;
    override public void _Ready() {
        this.leftName = GetNode<RichTextLabel>("NameLeft");
        this.rightName = GetNode<RichTextLabel>("NameRight");
        this.gif = GetNode<Sprite>("Sprite");
    }
    public override void _Process(float delta) {
        this.t += 1;
        if (this.t > 15) {
            this.t = 0;
            this.gif.Frame += 1;
        }
    }
    public void SetData(ChatMessageSide side, string name, Texture texture) {
        if (side == ChatMessageSide.Left) {
            this.rightName.Visible = false;
            this.leftName.BbcodeText = name;
        } else if (side == ChatMessageSide.Right) {
            this.leftName.Visible = false;
            this.rightName.BbcodeText = "[right]You[/right]";
        }
        this.gif.Texture = texture;
    }

    public override int GetHeight() {
        return this.gif.Texture.GetHeight() + 64;
    }
}
