using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public enum GameFlags {
    TestFlag
}

public static class GameManager {
    public static SSHWindow sshWindow;
    public static ChatWindow chatWindow;
    public static MapWindow mapWindow;
    public static FilesWindow filesWindow;
    public static ViewerWindow viewerWindow;
    public static OrdersWindow ordersWindow;

    public static Timeline timeline;

    public static string playerName;

    private static Dictionary<GameFlags, int> flags = new Dictionary<GameFlags, int> {
        { GameFlags.TestFlag, 0 }
    };
    public static void DroneCommand(string command) {
        sshWindow.AddText("[color=gray]$ " + command + "[/color]\n");
        var commandStrings = command.Split(' ');
        if (commandStrings.Length == 5 && commandStrings[0] == "drone" && commandStrings[1] == "mv") { // drone mv <drone> <x> <y>
            HandleMoveCommand(commandStrings[2], commandStrings[3], commandStrings[4]);
        } else if (commandStrings.Length == 2 && commandStrings[0] == "location" && commandStrings[1] == "ls") { // location ls
            HandleLocationsListCommand();
        } else if (commandStrings.Length == 2 && commandStrings[0] == "drone" && commandStrings[1] == "ls") { // drone ls
            HandleDroneListCommand();
        } else if (commandStrings.Length == 3 && commandStrings[0] == "location" && commandStrings[1] == "items") { // location items <location>
            HandleLocationItemsCommand(commandStrings[2]);
        } else if (commandStrings.Length == 4 && commandStrings[0] == "drone" && commandStrings[1] == "pickup") { // drone pickup <drone> <item>
            HandlePickupItem(commandStrings[2], commandStrings[3]);
        } else if (commandStrings.Length == 3 && commandStrings[0] == "drone" && commandStrings[1] == "drop") { // drone drop <drone>
            HandleDropItem(commandStrings[2]);
        } else {
            sshWindow.AddText(MakeErrorResponse("Invalid command or number of arguments. Refer to the manual for more information."));
        }
        sshWindow.AddText("\n");
    }
    private static string MakeErrorResponse(string response) {
        return "[color=red]Error: " + response + "[/color]";
    }
    private static void HandleMoveCommand(string droneName, string rawTargetX, string rawTargetY) {
        int targetX, targetY;
        if (!int.TryParse(rawTargetX, out targetX)) {
            sshWindow.AddText(MakeErrorResponse("X position must be an integer value."));
            return;
        }
        if (!int.TryParse(rawTargetY, out targetY)) {
            sshWindow.AddText(MakeErrorResponse("Y position must be an integer value."));
            return;
        }
        if ((targetX <= 0) || (targetX >= 100)) {
            sshWindow.AddText(MakeErrorResponse("X position out of bounds. Must be between 0 and 100."));
            return;
        }
        if ((targetY <= 0) || (targetY >= 100)) {
            sshWindow.AddText(MakeErrorResponse("Y position out of bounds. Must be between 0 and 100."));
            return;
        }
        Drone drone = DroneManager.GetDrone(droneName);
        if (drone == null) {
            sshWindow.AddText(MakeErrorResponse("Drone '" + droneName + "' not found."));
            return;
        }
        if (drone.target != Vector2.Zero) {
            sshWindow.AddText(MakeErrorResponse("Drone '" + droneName + "' is already moving. Wait for it to arrive to it's destination before moving it again."));
            return;
        }
        drone.target = new Vector2(targetX, targetY);
        sshWindow.AddText("Moving [color=green]" + droneName + "[/color] to (" + targetX + ", " + targetY + ")");
    }
    private static void HandleLocationsListCommand() {
        sshWindow.AddText("List of available locations:\n");
        sshWindow.AddText("\t[color=yellow]" + "Name".PadRight(16) + "X".PadRight(3) + "Y".PadRight(3) + "[/color]");
        foreach (var location in BuildingManager.GetBuildings()) {
            var namePadded = location.name.ToString().PadRight(16);
            var xPadded = location.position.X.ToString().PadRight(3);
            var yPadded = location.position.Y.ToString().PadRight(3);
            sshWindow.AddText("\n\t" + namePadded + xPadded + yPadded);
        }
    }
    private static void HandleDroneListCommand() {
        sshWindow.AddText("List of available drones:\n");
        sshWindow.AddText("\t[color=yellow]" + "Name".PadRight(16) + "X".PadRight(3) + "Y".PadRight(3) + "[/color]");
        foreach (var drone in DroneManager.GetDrones()) {
            var namePadded = drone.name.ToString().PadRight(16);
            var xPadded = drone.position.X.ToString().PadRight(3);
            var yPadded = drone.position.Y.ToString().PadRight(3);
            sshWindow.AddText("\n\t" + namePadded + xPadded + yPadded);
        }
    }
    private static void HandleLocationItemsCommand(string locationName) {
        var location = BuildingManager.GetBuilding(locationName);
        if (location == null) {
            sshWindow.AddText(MakeErrorResponse("Location '" + locationName + "' not found."));
            return;
        }
        sshWindow.AddText("Items at location [color=green]" + locationName + "[/color]:\n");
        sshWindow.AddText("\t[color=yellow]" + "Name".PadRight(16) + "Weight".PadRight(5) + "[/color]");
        foreach (var item in location.GetItems()) {
            var namePadded = item.name.ToString().PadRight(16);
            var weightPadded = item.weight.ToString().PadRight(3);
            sshWindow.AddText("\n\t" + namePadded + weightPadded);
        }
    }
    private static void HandlePickupItem(string droneName, string itemName) {
        var drone = DroneManager.GetDrone(droneName);
        if (drone == null) {
            sshWindow.AddText(MakeErrorResponse("Drone '" + droneName + "' not found."));
            return;
        }
        if (!drone.IsFree()) {
            sshWindow.AddText(MakeErrorResponse("Drone '" + droneName + "' is carrying an item already. Drop it before picking a new one up."));
            return;
        }
        var droneLocation = BuildingManager.GetBuildingAtPosition(drone.position);
        if (droneLocation == null) {
            sshWindow.AddText(MakeErrorResponse("Drone '" + droneName + "' is not at a location."));
            return;
        }
        var item = droneLocation.PopItemWithName(itemName);
        if (item == null) {
            sshWindow.AddText(MakeErrorResponse("Location '" + droneLocation.name + "' does not contain item '" + itemName + "'."));
            return;
        }
        drone.item = item;
        sshWindow.AddText("Picked up [color=green]" + item.name + "[/color] with drone [color=green]" + droneName + "[/color] at [color=green]" + droneLocation.name + "[/color].");
    }
    private static void HandleDropItem(string droneName) {
        var drone = DroneManager.GetDrone(droneName);
        if (drone == null) {
            sshWindow.AddText(MakeErrorResponse("Drone '" + droneName + "' not found."));
            return;
        }
        if (drone.IsFree()) {
            sshWindow.AddText(MakeErrorResponse("Drone '" + droneName + "' is not carrying an item."));
            return;
        }
        var droneLocation = BuildingManager.GetBuildingAtPosition(drone.position);
        if (droneLocation == null) {
            sshWindow.AddText(MakeErrorResponse("Drone '" + droneName + "' is not at a location."));
            return;
        }
        var item = drone.item;
        droneLocation.AddItem(item);
        drone.DropItem();
        sshWindow.AddText("Dropped [color=green]" + item.name + "[/color] with drone [color=green]" + droneName + "[/color] at [color=green]" + droneLocation.name + "[/color].");
    }
}

public static class DroneManager {
    private static Dictionary<string, Drone> drones = new Dictionary<string, Drone>();
    public static void AddDrone(Drone drone) {
        drones.Add(drone.name, drone);
        GameManager.mapWindow.AddDroneToMap(drone, "eagle");
    }
    public static Drone GetDrone(string droneName) {
        if (drones.ContainsKey(droneName)) { return drones[droneName]; }
        return null;
    }
    public static Drone[] GetDrones() {
        return drones.Values.ToArray();
    }
}

public class Drone {
    public string name;
    public float speed = 0.1f;
    public Item item;
    public Vector2 position;
    public Vector2 target = Vector2.Zero;
    public Drone(string name, Vector2 position) {
        this.name = name;
        this.position = position;
    }
    public void Update() {
        if (this.target == Vector2.Zero) { return; }
        if (this.target.X != this.position.X) { this.position.X += Math.Sign(this.target.X - this.position.X) * this.speed; }
        if (this.target.Y != this.position.Y) { this.position.Y += Math.Sign(this.target.X - this.position.X) * this.speed; }
        if (Math.Abs(this.target.X - this.position.X) < this.speed * 2) { this.position.X = this.target.X; }
        if (Math.Abs(this.target.Y - this.position.Y) < this.speed * 2) { this.position.Y = this.target.Y; }
        if (this.position == this.target) { this.target = Vector2.Zero; }
    }
    public bool IsFree() {
        return this.item == null;
    }
    public void DropItem() {
        this.item = null;
    }
}

public static class BuildingManager {
    private static Dictionary<string, Building> buildings = new Dictionary<string, Building>();
    public static void AddBuilding(Building building) {
        buildings.Add(building.name, building);
        GameManager.mapWindow.AddBuildiingToMap(building, "factory");
    }
    public static Building GetBuilding(string buildingName) {
        if (buildings.ContainsKey(buildingName)) { return buildings[buildingName]; }
        return null;
    }
    public static Building[] GetBuildings() {
        return buildings.Values.ToArray();
    }
    public static Building GetBuildingAtPosition(Vector2 position) {
        foreach (var building in buildings.Values) {
            if (building.position == position) { return building; }
        }
        return null;
    }

}

public class Building {
    public string name;
    public Vector2 position;
    public List<Item> items = new List<Item>();
    public Building(string name, Vector2 position) {
        this.name = name;
        this.position = position;
    }
    public void AddItem(Item item) {
        this.items.Add(item);
    }
    public Item[] GetItems() {
        return this.items.ToArray();
    }
    public void Update() { }
    public Item PopItemWithName(string name) {
        for (int i = 0; i < this.items.Count; i++) {
            if (this.items[i].name == name) {
                var item = this.items[i];
                this.items.RemoveAt(i);
                return item;
            }
        }
        return null;
    }
}

public class Item {
    public string name;
    public int weight;
    public Item(string name, int weight) {
        this.name = name;
        this.weight = weight;
    }
    public void Update() { }

}