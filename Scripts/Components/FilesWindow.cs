using Godot;
using System.Collections.Generic;

public enum FileType {
	Pdf,
	Image,
}

public class FilesWindow: BaseWindow {
	private readonly PackedScene fileIconScene = GD.Load<PackedScene>("res://Components/Files/FileIcon.tscn");

	private Control filesNode;
	private Dictionary<string, FileIcon> fileIcons = new Dictionary<string, FileIcon>();

	override public void _Ready() {
		GameManager.filesWindow = this;
		this.filesNode = GetNode<Control>("Files");
	}

	public override void _Process(float delta) {
		for (var i = 0; i < this.filesNode.GetChildCount(); i++) {
			var file = this.filesNode.GetChild<FileIcon>(i);
			var row = i % 6;
			var col = i / 6;
			file.RectPosition = new Vector2(row * 92, col * 128);
		}
	}

	public void AddPdfFile(string fileName, Texture texture) {
		var icon = fileIconScene.Instance<FileIcon>();
		this.filesNode.AddChild(icon);
		icon.SetData(fileName, FileType.Pdf, texture);
		fileIcons.Add(fileName, icon);
	}

}
