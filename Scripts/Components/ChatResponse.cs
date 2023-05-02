using Godot;

public class ChatResponse: Control {
    [Export]
    public int chatIndex;

    private bool enabled;

    private Control responseANode;
    private Control responseBNode;
    private Control responseCNode;

    private string responseA;
    private string responseB;
    private string responseC;

    private bool hoveringA;
    private bool hoveringB;
    private bool hoveringC;
    public override void _Ready() {
        this.responseANode = GetNode<Control>("Option1");
        this.responseBNode = GetNode<Control>("Option2");
        this.responseCNode = GetNode<Control>("Option3");

        this.responseANode.Connect("gui_input", this, nameof(HandleInputA));
        this.responseANode.Connect("mouse_entered", this, nameof(HandleMouseEnteredA));
        this.responseANode.Connect("mouse_exited", this, nameof(HandleMouseExitedA));

        this.responseBNode.Connect("gui_input", this, nameof(HandleInputB));
        this.responseBNode.Connect("mouse_entered", this, nameof(HandleMouseEnteredB));
        this.responseBNode.Connect("mouse_exited", this, nameof(HandleMouseExitedB));

        this.responseCNode.Connect("gui_input", this, nameof(HandleInputC));
        this.responseCNode.Connect("mouse_entered", this, nameof(HandleMouseEnteredC));
        this.responseCNode.Connect("mouse_exited", this, nameof(HandleMouseExitedC));

        CallDeferred(nameof(UpdatePosition));
    }
    public override void _Process(float delta) {
        UpdatePosition();
        if (this.hoveringA) {
            this.responseANode.Modulate = new Color("ff9999");
        } else {
            this.responseANode.Modulate = new Color("ffffff");
        }
        if (this.hoveringB) {
            this.responseBNode.Modulate = new Color("ff9999");
        } else {
            this.responseBNode.Modulate = new Color("ffffff");
        }
        if (this.hoveringC) {
            this.responseCNode.Modulate = new Color("ff9999");
        } else {
            this.responseCNode.Modulate = new Color("ffffff");
        }
    }
    private void UpdatePosition() {
        if (GameManager.chatWindow.activeChat == this.chatIndex && this.enabled) { this.Visible = true; } else { this.Visible = false; }
    }
    public void SetResponses(string responseA, string responseB, string responseC) {
        this.Visible = true;
        this.enabled = true;
        this.responseA = responseA;
        this.responseB = responseB;
        this.responseC = responseC;
        if (responseA != null) {
            this.responseANode.GetNode<RichTextLabel>("Text").BbcodeText = "[center]" + responseA + "[/center]";
            this.responseANode.Visible = true;
        } else { this.responseANode.Visible = false; }
        if (responseB != null) {
            this.responseBNode.GetNode<RichTextLabel>("Text").BbcodeText = "[center]" + responseB + "[/center]";
            this.responseBNode.Visible = true;
        } else { this.responseBNode.Visible = false; }
        if (responseC != null) {
            this.responseCNode.GetNode<RichTextLabel>("Text").BbcodeText = "[center]" + responseC + "[/center]";
            this.responseCNode.Visible = true;
        } else { this.responseCNode.Visible = false; }
    }

    private void HandleMouseEnteredA() {
        this.hoveringA = true;
    }
    private void HandleMouseExitedA() {
        this.hoveringA = false;
    }
    private void HandleMouseEnteredB() {
        this.hoveringB = true;
    }
    private void HandleMouseExitedB() {
        this.hoveringB = false;
    }
    private void HandleMouseEnteredC() {
        this.hoveringC = true;
    }
    private void HandleMouseExitedC() {
        this.hoveringC = false;
    }

    private void HandleInputA(InputEvent inputEvent) {
        if (inputEvent == null) { return; }
        if (inputEvent is InputEventMouseButton buttonEvent) {
            if (buttonEvent.IsActionPressed("mouse_left") && this.hoveringA) {
                GameManager.chatWindow.GetChat((ChatConversation)this.chatIndex).AddChatMessage(ChatMessageSide.Right, "You", this.responseA, null);
                GameManager.chatWindow.GetChat((ChatConversation)this.chatIndex).SetLastResponse(1);
                this.Visible = false;
                this.enabled = false;
            }
        }
    }
    private void HandleInputB(InputEvent inputEvent) {
        if (inputEvent == null) { return; }
        if (inputEvent is InputEventMouseButton buttonEvent) {
            if (buttonEvent.IsActionPressed("mouse_left") && this.hoveringB) {
                GameManager.chatWindow.GetChat((ChatConversation)this.chatIndex).AddChatMessage(ChatMessageSide.Right, "You", this.responseB, null);
                GameManager.chatWindow.GetChat((ChatConversation)this.chatIndex).SetLastResponse(2);
                this.Visible = false;
                this.enabled = false;
            }
        }
    }
    private void HandleInputC(InputEvent inputEvent) {
        if (inputEvent == null) { return; }
        if (inputEvent is InputEventMouseButton buttonEvent) {
            if (buttonEvent.IsActionPressed("mouse_left") && this.hoveringC) {
                GameManager.chatWindow.GetChat((ChatConversation)this.chatIndex).AddChatMessage(ChatMessageSide.Right, "You", this.responseC, null);
                GameManager.chatWindow.GetChat((ChatConversation)this.chatIndex).SetLastResponse(3);
                this.Visible = false;
                this.enabled = false;
            }
        }
    }
}
