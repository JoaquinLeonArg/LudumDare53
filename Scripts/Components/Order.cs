using Godot;
using System.Collections.Generic;
using System.Linq;

public class Order: Control {
    private Control orderItemsNode;

    private Building destination;
    private List<string> itemNames = new List<string>();
    private List<bool> completedItems = new List<bool>();

    public override void _Ready() {
        this.orderItemsNode = GetNode<Control>("Items");
    }
    public void SetData(string destinationName, string[] itemNames) {
        this.destination = BuildingManager.GetBuilding(destinationName);
        this.itemNames = itemNames.ToList();
        this.GetNode<RichTextLabel>("Destination").BbcodeText = "[center]" + destinationName + "[/center]";
        for (var i = 0; i < itemNames.Length; i++) {
            var itemName = itemNames[i];
            var orderItem = this.orderItemsNode.GetChild<OrderItem>(i);
            orderItem.Visible = true;
            orderItem.GetNode<RichTextLabel>("ItemName").Text = itemName;
            orderItem.GetNode<Sprite>("Checkmark").Visible = false;
            completedItems.Add(false);
        }
    }
    public override void _Process(float delta) {
        for (var i = 0; i < this.itemNames.Count(); i++) {
            var itemName = this.itemNames[i];
            foreach (var item in this.destination.GetItems()) {
                if (item.name == itemName && !this.completedItems[i]) {
                    this.destination.PopItemWithName(itemName);
                    this.completedItems[i] = true;
                    this.orderItemsNode.GetChild<OrderItem>(i).GetNode<Sprite>("Checkmark").Visible = true;
                }
            }
        }
        if (completedItems.All(item => item == true)) { this.QueueFree(); } // TODO: Animate or something?
    }
}
