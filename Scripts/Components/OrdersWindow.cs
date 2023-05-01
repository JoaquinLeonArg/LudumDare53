using Godot;
using System.Collections.Generic;

public class OrdersWindow: BaseWindow {
	private readonly PackedScene orderScene = GD.Load<PackedScene>("res://Components/Orders/Order.tscn");

	private Control ordersNode;
	private RichTextLabel emptyNode;

	private List<Order> orders = new List<Order>();
	public override void _Ready() {
		GameManager.ordersWindow = this;
		this.ordersNode = GetNode<Control>("Orders");
		this.emptyNode = GetNode<RichTextLabel>("NoOrders");
		Test();
	}

	private void Test() {
		this.AddOrder("Factory", new string[] { "Mate", "Pepe" });
		this.AddOrder("Factory", new string[] { "Pupu", "OtroItemAca" });
	}

	public void AddOrder(string destination, string[] itemNames) {
		var order = orderScene.Instance<Order>();
		order.SetSize(new Vector2(380, 172));
		this.ordersNode.AddChild(order);
		this.orders.Add(order);
		order.SetData(destination, itemNames);
		this.UpdateLayout();
	}


	private void UpdateLayout() {
		if (this.ordersNode.GetChildCount() == 0) { this.emptyNode.Visible = true; } else { this.emptyNode.Visible = false; };
		for (var i = 0; i < this.ordersNode.GetChildCount(); i++) {
			this.ordersNode.GetChild<Order>(i).SetPosition(new Vector2(8, i * 176));
		}
	}
}
