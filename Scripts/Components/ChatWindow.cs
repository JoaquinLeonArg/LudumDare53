using Godot;
using System.Collections.Generic;

public enum ChatConversation {
	Boss = 0,
	Lara = 1,
	Charlie = 2
}

public class ChatWindow: BaseWindow {
	private readonly PackedScene chatListItemScene = GD.Load<PackedScene>("res://Components/Sluck/ChatListItem.tscn");
	private readonly PackedScene chatMessageListScene = GD.Load<PackedScene>("res://Components/Sluck/ChatMessageList.tscn");

	private Control chatListNode;
	private Control chatMessagesNode;
	private Control profileNode;
	private Control responsesNode;

	public int activeChat;
	private bool sound;
	private Dictionary<string, ChatListItem> chatList = new Dictionary<string, ChatListItem>();
	private Dictionary<string, ChatMessageList> chatMessages = new Dictionary<string, ChatMessageList>();
	private List<string> names = new List<string> { "Alon Mosk", "L.A.R.A.", "Charlie" };
	private List<string> descriptions = new List<string> { "Chief Executive Something", "AI Assistant", "Friendly coworker" };
	private List<Texture> profileImages = new List<Texture> {
		ResourceLoader.Load<Texture>("res://Resources/Image/portrait_boss2.png"),
		ResourceLoader.Load<Texture>("res://Resources/Image/portrait_lara.png"),
		ResourceLoader.Load<Texture>("res://Resources/Image/portrait_charlie2.png")
	};
	public override void _Ready() {
		GameManager.chatWindow = this;
		this.chatListNode = GetNode<Control>("ChatList");
		this.chatMessagesNode = GetNode<Control>("ChatMessages");
		this.profileNode = GetNode<Control>("Profile");
		this.responsesNode = GetNode<Control>("Responses");
		this.AddChatToList(0);
		SetActiveChat(0);
		this.sound = true; // Hack to disable the sound the first time
	}

	override public void Open(Vector2 from) {
		base.Open(from);
	}

	public void AddChatToList(int index) {
		var name = this.names[index];
		// Add chat entry to left side
		var chatListItem = chatListItemScene.Instance<ChatListItem>();
		chatListItem.RectPosition = new Vector2(0, 48 * this.chatListNode.GetChildCount());
		chatListItem.SetChatName(name);
		this.chatListNode.AddChild(chatListItem);
		this.chatList.Add(name, chatListItem);
		chatListItem.chatIndex = this.chatList.Count - 1;

		// Add chat history
		var chatMessageList = chatMessageListScene.Instance<ChatMessageList>();
		chatMessageList.MouseFilter = MouseFilterEnum.Pass;
		chatMessageList.Visible = false;
		chatMessageList.chatIndex = index;
		this.chatMessagesNode.AddChild(chatMessageList);
		this.chatMessages.Add(name, chatMessageList);

	}

	public void SetActiveChat(int index) {
		if (this.sound) GetNode<AudioStreamPlayer>("SelectSound").Play();
		this.activeChat = index;
		for (var i = 0; i < this.chatListNode.GetChildCount(); i++) {
			if (i == index) {
				this.chatMessagesNode.GetChild<ChatMessageList>(i).Visible = true;
				this.chatListNode.GetChild<ChatListItem>(i).active = true;
			} else {
				this.chatMessagesNode.GetChild<ChatMessageList>(i).Visible = false;
				this.chatListNode.GetChild<ChatListItem>(i).active = false;
			}
		}
		this.profileNode.GetNode<RichTextLabel>("Name").Text = this.names[index];
		this.profileNode.GetNode<RichTextLabel>("Title").Text = this.descriptions[index];
		this.profileNode.GetNode<Sprite>("ProfileImage").Texture = this.profileImages[index];
	}
	public ChatMessageList GetChat(ChatConversation conversation) {
		return this.chatMessages[names[((int)conversation)]];
	}
	public ChatResponse GetChatResponse(ChatConversation conversation) {
		return this.responsesNode.GetChild<ChatResponse>((int)conversation);
	}
	public ChatListItem GetChatListItem(int index) {
		return this.chatListNode.GetChild<ChatListItem>(index);
	}
}
