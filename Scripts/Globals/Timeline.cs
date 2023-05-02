using Godot;
using System.Threading.Tasks;

public class Timeline: Node {
    private ChatMessageList bossChat;
    private ChatResponse bossChatResponse;
    private ChatMessageList laraChat;
    private ChatResponse laraChatResponse;
    private ChatMessageList charlieChat;
    private ChatResponse charlieChatResponse;

    private string bossName = "Alon Mosk";
    private string laraName = "L.A.R.A.";
    private string charlieName = "Charlie";
    public void SetData() {
        bossChat = GameManager.chatWindow.GetChat(ChatConversation.Boss);
        bossChatResponse = GameManager.chatWindow.GetChatResponse(ChatConversation.Boss);
    }
    public override void _Ready() {
        GameManager.timeline = this;
    }
    public override void _Process(float delta) {
        DroneManager.Update();
    }
    private SignalAwaiter WaitFor(float seconds) { return ToSignal(GetTree().CreateTimer(seconds), "timeout"); }
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

        var finalCountdown = GetTree().CurrentScene.GetNode<Control>("Overlays/CountDown");
        var finalCountdownLabel = finalCountdown.GetNode<RichTextLabel>("Time");
        var finalFx = GetTree().CurrentScene.GetNode<Control>("Overlays/FinalFx");

        // Boss intro
        await WaitFor(5);
        if (!GameManager.chatWindow.Visible) GameManager.chatWindow.Open(GameManager.chatWindow.RectGlobalPosition);
        await WaitFor(2);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Hey " + GameManager.playerName + "!", null);
        await WaitFor(5);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Welcome to ZonMe Inc, engineer! We are a multinational delivery company committed to providing exceptional service to our customers.", null);
        await WaitFor(5);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Our mission is to ensure that every package we deliver arrives on time and in perfect condition, no matter where it is going.", null);
        await WaitFor(5);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "At ZonMe, we value teamwork, integrity, and a dedication to excellence. We're thrilled to have you join our team and look forward to building a strong working relationship with you.", null);
        await WaitFor(2);
        bossChatResponse.SetResponses("Hi?", "Well..", "Who are you again?");
        var bossResponse = await WaitForResponse(bossChat);
        await WaitFor(3);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Today is your first day as a trainee, so don't worry if you don't know everything yet. Take your time to learn everything about our system, and if you have any questions, just ask L.A.R.A. She's our friendly Sluck bot, and she's always happy to help.", null);
        await WaitFor(10);

        // Lara intro
        GameManager.chatWindow.AddChatToList((int)ChatConversation.Lara);
        laraChat = GameManager.chatWindow.GetChat(ChatConversation.Lara);
        laraChatResponse = GameManager.chatWindow.GetChatResponse(ChatConversation.Lara);

        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Hi, I'm [b]L.A.R.A.[/b]. I'm just a bot and I don't have time for nonsense. But if you have questions about the job, I guess I can help you. Don't expect me to treat you with kid gloves, though.", null);
        laraChatResponse.SetResponses("Hi L.A.R.A.!", "L.A.R.A.?", null);
        var laraResponse = await WaitForResponse(laraChat);
        if (laraResponse == 2) {
            laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "It stands for [b]'Logistics Assistance and Resource Aid'[/b].", null);

        }
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "It's your first day as a [b]Drone Engineer[/b]. Your job is to manage the drones and ensure that deliveries are made on time. You'll be in charge of controlling the drones to deliver a variety of items, from mangas and dinosaurs, to [b]your own dignity[/b].", null);
        await WaitFor(8);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "I don't really care if you succeed or fail, but you better give it your best shot anyway :$", null);
        await WaitFor(5);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Well, I guess I'll explain it to you since you're hopeless. On your screen, you'll find everything you need, like [b]Files[/b] and [b]Sluck[/b].", null);
        await WaitFor(3);

        // Lara DroneMap tutorial
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Let me enable [b]DroneMap[/b] for you.", null);
        await WaitFor(2);
        GetTree().CurrentScene.GetNode<Icon>("Icons/DroneMap").ShowIcon();
        await WaitFor(1);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "There, you should be able to open [b]DroneMap[/b] by double clicking the icon on your desktop.", null);
        while (true) {
            if (GameManager.mapWindow.Visible) break;
            await WaitFor(1);
        }
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Good!", null);
        await WaitFor(3);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "[b]DroneMap[/b] allows you to monitor the drones in real-time. Let's give you control of a drone now.", null);
        await WaitFor(2);
        DroneManager.AddDrone(new Drone("eagle", new System.Numerics.Vector2(20, 30), 0.006f, 20), "eagle");
        await WaitFor(1);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "I've added [b]eagle[/b] to your fleet. You should be able to see it on the map now.", null);
        laraChatResponse.SetResponses("I can see it.", "What?", null);
        laraResponse = await WaitForResponse(laraChat);
        await WaitFor(1);
        if (laraResponse == 2) {
            laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "It's the eagle icon on your [b]DroneMap[/b] screen.", null);
            await WaitFor(2);
        }
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Let's see how you can actually control your drone now, shall we?", null);
        await WaitFor(2);

        // Lara Files and manual tutorial
        GameManager.filesWindow.AddPdfFile("manual_v1", ResourceLoader.Load<Texture>("res://Resources/Image/pdf_manual.png"));
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "I've sent you a new file, you should see it on your [b]Files[/b] window.", null);
        laraChatResponse.SetResponses("Which one?", "Should I open it?", null);
        laraResponse = await WaitForResponse(laraChat);
        await WaitFor(1);
        if (laraResponse == 1) {
            laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "[b]manual_v1[/b], should be in your Files window. You can open that using the icon on your desktop if you havent already.", null);
        } else {
            laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Well, I sent it to you for a reason...", null);
        }
        while (true) {
            if (GameManager.viewerWindow.Visible && GameManager.viewerWindow.contentName == "manual_v1") break;
            await WaitFor(1);
        }
        await WaitFor(1);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "What took you so long? Maybe we should fire you and find a replacement.", null);
        await WaitFor(2);

        // Lara DroneMgr tutorial
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Anyways, use that document if you ever forget your [b]DroneMgr[/b] commands.", null);
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Speaking of which, I'll enable [b]DroneMgr[/b] for you so we can start working already.", null);
        await WaitFor(2);
        GetTree().CurrentScene.GetNode<Icon>("Icons/DroneMgr").ShowIcon();
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "There you go. Open it. You should know where to find it by now.", null);
        while (true) {
            if (GameManager.sshWindow.Visible) break;
            await WaitFor(1);
        }
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "[b]DroneMgr[/b] is where you can input commands to control the drones. Let's start with an easy command.", null);
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Click on the bottom of the screen, next to the '>' icon. Then write [b]drone ls[/b] and hit enter.", null);
        await WaitFor(1);
        while (true) {
            if (GameManager.flags[GameFlags.ListedDrones] == 1) break;
            await WaitFor(1);
        }
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Good. You can see the list of drones under your control. In this case, it's just [b]eagle[/b].", null);
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Let's do something useful with it. To move a drone, write [b]drone mv[/b], the drone name, and the coordinates you want it to go to.", null);
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Keep in mind coordinates on the map range from 0 to 100. Trying to move a drone outside the map will result in an error.", null);
        await WaitFor(3);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "And we'll fire you.", null);
        await WaitFor(3);
        laraChatResponse.SetResponses("Wait, for real?", "I guess I'll be careful", null);
        laraResponse = await WaitForResponse(laraChat);
        if (laraResponse == 1) {
            laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "No.", null);
        } else {
            laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Hm.", null);
        }
        await WaitFor(1);
        var t = 0;
        while (true) {
            if (GameManager.flags[GameFlags.MovedDrone] == 1) break;
            await WaitFor(1);
            t++;
            if (t == 10) { laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Try moving [b]eagle[/b] now. Something like [b]drone mv eagle 10 30[/b] should do it.", null); }
        }
        if (t == 0) { laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Fast learner, huh?", null); }
        await WaitFor(1);

        // Lara locations tutorial
        BuildingManager.AddBuilding(new Building("hq", new System.Numerics.Vector2(17, 34)), "hq");
        BuildingManager.AddBuilding(new Building("cotton_factory", new System.Numerics.Vector2(39, 60)), "factory");
        BuildingManager.GetBuilding("hq").AddItem(new Item("sugar", 1));
        BuildingManager.GetBuilding("hq").AddItem(new Item("ramen_noodles", 1));
        BuildingManager.GetBuilding("hq").AddItem(new Item("3d_printer", 15));
        BuildingManager.GetBuilding("hq").AddItem(new Item("cat_photos", 2));


        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Take a look at the map now. I've registered a few locations of interest.", null);
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "To see a list of all locations on [b]DroneMgr[/b], use the [b]location ls[/b] command.", null);
        while (true) {
            if (GameManager.flags[GameFlags.ListedLocations] == 1) break;
            await WaitFor(1);
        }
        await WaitFor(1);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "You can see each location's name, as well as it's coordinates.", null);
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Move [b]eagle[/b] to [b]hq[/b] now.", null);
        t = 0;
        while (true) {
            if (DroneManager.GetDrone("eagle").position == BuildingManager.GetBuilding("hq").position) break;
            await WaitFor(1);
            t++;
            if (t == 20) { laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Remember, to move a drone you need to use [b]drone mv <drone> <x> <y>[/b].", null); }
            if (t == 40) { laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "You can see where [b]hq[/b] is located by using [b]location ls[/b].", null); }
            if (t == 60) { laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Is it really that hard? [b]drone mv eagle 17 34[/b] on DroneMgr, which you can find on your desktop.", null); }
        }
        await WaitFor(1);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Now that your drone is at hq, you might want to take a look at what items we have in stock.", null);
        await WaitFor(3);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Now that your drone is at hq, you might want to take a look at what items we have in stock.", null);
        await WaitFor(2);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "[b]item ls <location>[/b] to list all items in a particular location.", null);
        t = 0;
        while (true) {
            if (GameManager.flags[GameFlags.HQItems] == 1) break;
            await WaitFor(1);
            t++;
            if (t == 30) { laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "[b]item ls hq[/b]. Come on!", null); }
        }
        await WaitFor(2);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "How's the onboarding process going?", null);
        await WaitFor(3);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "You should be receiving your first order soon.", null);
        bossChatResponse.SetResponses("Going well", "L.A.R.A. is mean", null);
        bossResponse = await WaitForResponse(bossChat);
        await WaitFor(2);
        if (bossResponse == 1) { bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Glad to hear that.", null); }
        if (bossResponse == 2) { bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "I don't really care.", null); }
        await WaitFor(3);
        GetTree().CurrentScene.GetNode<Icon>("Icons/OrderMgr").ShowIcon();
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "[b]ZoneMeReq[/b] should be in your desktop. Open it.", null);
        while (true) {
            if (GameManager.ordersWindow.Visible) break;
            await WaitFor(1);
        }
        await WaitFor(2);

        // Tutorial order
        GameManager.ordersWindow.AddOrder("cotton_factory", new string[] { "sugar" });
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Looks like they need some [b]sugar[/b] at [b]cotton_factory[/b]", null);
        await WaitFor(3);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Grab some from [b]hq[/b] with [b]eagle[/b] using [b]drone pickup eagle sugar[/b], move it to the [b]cotton_factory[/b], then drop it there using [b]drone drop eagle[/b].", null);
        while (true) {
            if (GameManager.ordersWindow.GetOrders().Count == 0) break;
            await WaitFor(1);
        }
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Good job.", null);
        await WaitFor(3);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "I guess.", null);
        await WaitFor(15);

        // First real order
        BuildingManager.AddBuilding(new Building("drone_factory", new System.Numerics.Vector2(72, 10)), "factory");
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("drone_parts", 5));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("drone_parts", 5));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("drone_parts", 5));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("drone_parts", 5));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("components_box", 80));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("components_box", 80));
        GameManager.ordersWindow.AddOrder("hq", new string[] { "drone_parts", "drone_parts" });
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "A new order arrived. Let's see if you can fulfill this one by yourself.", null);
        await WaitFor(3);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Remember: [b]each drone can carry only one item, and it must be at a location in order to pickup and drop items.[/b].", null);
        await WaitFor(3);
        t = 0;
        while (true) {
            if (GameManager.ordersWindow.GetOrders().Count == 0) break;
            t++;
            if (t == 40) { bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "HQ is complaining they haven't received their order yet. What's taking you so long?", null); }
            if (t == 80) { laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Need help with the order? You need to deliver two [b]drone_part[/b]s to hq. You can find them in the new [b]drone_factory[/b] I added to your map.", null); }
            if (t == 120) { laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Remember you can always refer to the manual in your [b]Files[/b] if you forgot a command.", null); }
            await WaitFor(1);
        }
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Well done.", null);
        await WaitFor(1);

        // Charlie is here!
        GameManager.chatWindow.AddChatToList((int)ChatConversation.Charlie);
        charlieChat = GameManager.chatWindow.GetChat(ChatConversation.Charlie);
        charlieChatResponse = GameManager.chatWindow.GetChatResponse(ChatConversation.Charlie);
        charlieChat.AddChatMessage(ChatMessageSide.Left, charlieName, "Oi mate! I have a [b]gossip...[/b]", null);
        charlieChatResponse.SetResponses("Who are you?", "What is it?", "Are you british?");
        var charlieResponse = await WaitForResponse(charlieChat);
        await WaitFor(2);
        if (charlieResponse == 1) {
            charlieChat.AddChatMessage(ChatMessageSide.Left, charlieName, "My name is [b]Dmitri Volkov[/b], I'm a prestigious bomb engineer.", null);
            await WaitFor(0.5f);
            charlieChat.AddChatMessage(ChatMessageSide.Left, charlieName, "I'm mean... I'm Charlie, your BRITISH coworker, mate. Fancy some tea?", null);
        }
        if (charlieResponse == 3) {
            charlieChat.AddChatMessage(ChatMessageSide.Left, charlieName, "How did you guess?", null);
        }
        await WaitFor(5);

        charlieChat.AddChatMessage(ChatMessageSide.Left, charlieName, "Alright mate, listen up. Apparently, the sales manager had a right go at our manager today. And get this, I reckon he completely forgot that we're a team of two working in this area. Can you believe it?", null);

        // New order
        await WaitFor(30);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "We have a new contract with the big corporations at the capital. You know what that means.", null);
        await WaitFor(5);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "New orders coming right up.", null);
        await WaitFor(15);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "HQ used the drone parts you brought earlier to make you a new drone. This one should be able to carry heavier items, but it's a bit slower.", null);
        DroneManager.AddDrone(new Drone("robin", new System.Numerics.Vector2(20, 30), 0.003f, 100), "robin");

        BuildingManager.AddBuilding(new Building("country_capital", new System.Numerics.Vector2(50, 50)), "city");
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("drone_parts", 5));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("drone_parts", 5));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("components_box", 80));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("screws", 1));
        BuildingManager.GetBuilding("drone_factory").AddItem(new Item("broken_drone", 10));
        BuildingManager.GetBuilding("country_capital").AddItem(new Item("documents", 1));

        await WaitFor(10);

        GameManager.ordersWindow.AddOrder("country_capital", new string[] { "components_box", "drone_parts", });
        GameManager.ordersWindow.AddOrder("hq", new string[] { "documents" });

        while (true) {
            if (GameManager.ordersWindow.GetOrders().Count == 0) break;
            await WaitFor(1);
        }


        // Final mission
        charlieChat.AddChatMessage(ChatMessageSide.Left, charlieName, "Looks like an important order is coming, mate. Be prepared.", null);
        await WaitFor(10);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "We are about to receive an important order from the government. BE FAST.", null);
        await WaitFor(5);
        bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "If you don't fulfill this one on time, the future of the country might be in danger.", null);
        await WaitFor(10);
        DroneManager.AddDrone(new Drone("heron", new System.Numerics.Vector2(70, 40), 0.01f, 200), "heron");
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "I've added a new drone to your fleet. Check it out.", null); ;
        await WaitFor(20);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "New items are ready at all locations. Check them out as well.", null);
        await WaitFor(20);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Once you are ready, let me know. You'll have around 5 minutes to complete the orders.", null);

        laraChatResponse.SetResponses("I'm ready.", null, null);
        laraResponse = await WaitForResponse(laraChat);
        await WaitFor(5);

        finalFx.Visible = true;
        finalCountdown.Visible = true;

        var won = false;
        while (true) {
            if (GameManager.ordersWindow.GetOrders().Count == 0) { won = true; break; }
            if (GameManager.finalCountdown <= 0) { won = false; break; }
            GameManager.finalCountdown--;
            finalCountdownLabel.BbcodeText = "[center]" + GameManager.finalCountdown + "[/center]";
            await WaitFor(1);
        }

        finalFx.Visible = false;
        finalCountdown.Visible = false;

        if (won) {
            if (GameManager.finalCountdown > 100) {
                bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Wow. I'm impressed.", null);
                bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Good job, expect to hear about a promotion soon.", null);
            } else if (GameManager.finalCountdown > 50) {
                bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "Good job. You can keep your job.", null);
            } else {
                bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "That was really close. But you did it in time, so... eh.", null);
            }

        } else {
            bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "You screwed up.", null);
            bossChat.AddChatMessage(ChatMessageSide.Left, bossName, "YOU'RE FIRED.", null);
        }
        await WaitFor(5);
        laraChat.AddChatMessage(ChatMessageSide.Left, laraName, "Thank you for playing. Game created for Ludum Dare 53. Make sure to check the credits. You know where to find them ;)", null);
    }
}
