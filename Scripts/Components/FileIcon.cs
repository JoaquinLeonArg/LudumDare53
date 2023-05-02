using Godot;
using System.Collections.Generic;

public class FileIcon: Control {
    public Dictionary<FileType, Texture> fileIconTextures = new Dictionary<FileType, Texture> {
        { FileType.Pdf, ResourceLoader.Load<Texture>("res://Resources/Image/file_pdf_icon.png") },
    };
    private Sprite iconNode;
    private RichTextLabel nameNode;
    private ColorRect selectedNode;
    private BaseWindow windowNode;
    private int doubleClickTimer;
    private bool selected;
    private Texture texture;
    private string fileName;
    public override void _Ready() {
        this.iconNode = GetNode<Sprite>("Icon");
        this.nameNode = GetNode<RichTextLabel>("Name");
        this.selectedNode = GetNode<ColorRect>("Selected");
        this.RectSize = new Vector2(88, 120); // Godot why
        Connect("gui_input", this, nameof(HandleGuiInput));
    }
    public void SetData(string fileName, FileType fileType, Texture texture) {
        this.fileName = fileName;
        this.iconNode.Texture = this.fileIconTextures[fileType];
        this.nameNode.BbcodeText = "[center]" + fileName + "[/center]";
        this.texture = texture;
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
                if (!GameManager.viewerWindow.Visible) {
                    GameManager.viewerWindow.SetContent(this.texture);
                    GameManager.viewerWindow.contentName = this.fileName;
                    GameManager.viewerWindow.Open(GameManager.viewerWindow.RectGlobalPosition);
                }
                this.doubleClickTimer = 0;
            }
            if (buttonEvent.IsActionPressed("mouse_left") && this.doubleClickTimer == 0) { // First click
                foreach (var genericIcon in GetParent().GetChildren()) {
                    var icon = genericIcon as FileIcon;
                    icon.selected = false;
                }
                this.selected = true;
                this.doubleClickTimer = 60; // 1 second to click again
            }
        }
    }
}
