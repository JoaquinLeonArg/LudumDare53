using Godot;
using System.Threading.Tasks;

public class Timeline: Node {
    private ChatMessageList bossChat;
    private ChatResponse bossChatResponse;
    private ChatMessageList laraChat;
    private ChatResponse laraChatResponse;
    private string bossName = "Your boss";
    private string laraName = "L.A.R.A.";
    public void SetData() {
        bossChat = GameManager.chatWindow.GetChat(ChatConversation.Boss);
        bossChatResponse = GameManager.chatWindow.GetChatResponse(ChatConversation.Boss);
    }
    public override void _Ready() {
        GameManager.timeline = this;
    }
    private SignalAwaiter WaitFor(float seconds) { return ToSignal(GetTree().CreateTimer(1.0f), "timeout"); }
    private async Task<int> WaitForResponse(ChatMessageList node) {
        int res;
        while (true) {
            res = node.GetAndResetLastResponse();
            if (res != 0) break;
            await WaitFor(1);
        }
        return res;
    }
    public async void Start() {
        CallDeferred(nameof(SetData));

        // Boss intro
        await WaitFor(5);
        GameManager.chatWindow.Open(GameManager.chatWindow.RectGlobalPosition);
        await WaitFor(2);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Hey " + GameManager.playerName + "!", null);
        await WaitFor(5);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Welcome to ZonMe Inc, engineer! We are a multinational delivery company committed to providing exceptional service to our customers.", null);
        await WaitFor(5);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Our mission is to ensure that every package we deliver arrives on time and in perfect condition, no matter where it is going.", null);
        await WaitFor(5);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "At ZonMe, we value teamwork, integrity, and a dedication to excellence. We're thrilled to have you join our team and look forward to building a strong working relationship with you.", null);
        await WaitFor(8);
        bossChatResponse.SetResponses("Hi?", "Well..", "Who are you again?");
        var bossResponse = await WaitForResponse(bossChat);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Today is your first day as a trainee, so don't worry if you don't know everything yet. Take your time to learn everything about our system, and if you have any questions, just ask LARA. She's our friendly Slack bot, and she's always happy to help.", null);
        await WaitFor(10);

        // Lara intro
        GameManager.chatWindow.AddChatToList(2);
        laraChat = GameManager.chatWindow.GetChat(ChatConversation.Lara);
        laraChatResponse = GameManager.chatWindow.GetChatResponse(ChatConversation.Lara);

        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Hi, I'm [b]LARA[/b]. I'm just a bot and I don't have time for nonsense. But if you have questions about the job, I guess I can help you. Don't expect me to treat you with kid gloves, though.", null);
        laraChatResponse.SetResponses("Hi LARA!", "What's this job about?", null);
        var laraResponse = await WaitForResponse(laraChat);
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "It's your first day as a [b]Drone Engineer[/b]. Your job is to manage the drones and ensure that deliveries are made on time. You'll be in charge of controlling the drones to deliver a variety of items, from mangas and dinosaurs, to [b]your own dignity[/b].", null);
        await WaitFor(5);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "I don't really care if you succeed or fail, but you better give it your best shot anyway :$", null);

    }
}
