using Godot;
using System.Collections.Generic;

public class OrdersWindow: BaseWindow {
    private readonly PackedScene orderScene = GD.Load<PackedScene>("res://Components/Orders/Order.tscn");

    private Control ordersNode;
    private RichTextLabel emptyNode;

    private int orderCount;

    private List<Order> orders = new List<Order>();
    public override void _Ready() {
        GameManager.ordersWindow = this;
        this.ordersNode = GetNode<Control>("Orders");
        this.emptyNode = GetNode<RichTextLabel>("NoOrders");
    }

    public void AddOrder(string destination, string[] itemNames) {
        GetNode<AudioStreamPlayer>("NewSound").Play();
        this.orderCount++;
        var order = orderScene.Instance<Order>();
        order.SetSize(new Vector2(380, 172));
        this.ordersNode.AddChild(order);
        this.orders.Add(order);
        order.SetData(destination, itemNames, orderCount);
    }


    override public void _Process(float delta) {
        if (this.ordersNode.GetChildCount() == 0) { this.emptyNode.Visible = true; } else { this.emptyNode.Visible = false; };
        for (var i = 0; i < this.ordersNode.GetChildCount(); i++) {
            this.ordersNode.GetChild<Order>(i).SetPosition(new Vector2(8, i * 176));
        }
    }
    public List<Order> GetOrders() {
        return this.orders;
    }
    public void ClearOrder(Order order) {
        this.orders.Remove(order);
    }
}
