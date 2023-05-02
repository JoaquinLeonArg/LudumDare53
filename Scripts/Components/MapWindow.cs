using Godot;
using System.Collections.Generic;



public class MapWindow: BaseWindow {
    private Control dronesNode;
    private Dictionary<string, Sprite> droneIcons = new Dictionary<string, Sprite>();
    public Dictionary<string, Texture> droneIconTextures = new Dictionary<string, Texture> {
        { "eagle", ResourceLoader.Load<Texture>("res://Resources/Image/eagle_Icongeek26.png") },
        { "heron", ResourceLoader.Load<Texture>("res://Resources/Image/heron_Icongeek26.png") },
        { "robin", ResourceLoader.Load<Texture>("res://Resources/Image/robin_Icongeek26.png") },
    };
    private Control buildingsNode;
    private Dictionary<string, Sprite> buildingIcons = new Dictionary<string, Sprite>();
    public Dictionary<string, Texture> buildingIconTextures = new Dictionary<string, Texture> {
        { "factory", ResourceLoader.Load<Texture>("res://Resources/Image/factory_Kiranshastry.png") },
        { "hq", ResourceLoader.Load<Texture>("res://Resources/Image/hq_Kiranshastry.png") },
        { "house", ResourceLoader.Load<Texture>("res://Resources/Image/house_Kiranshastry.png") },
        { "city", ResourceLoader.Load<Texture>("res://Resources/Image/city_Kiranshastry.png") }
    };
    override public void _Ready() {
        GameManager.mapWindow = this;
        this.dronesNode = GetNode<Control>("Drones");
        this.buildingsNode = GetNode<Control>("Buildings");
    }

    public override void _Process(float delta) {
        foreach (var drone in DroneManager.GetDrones()) {
            drone.Update();
            this.droneIcons[drone.name].Position = LogicToMapPosition(drone.position);
        }
    }

    public void AddDroneToMap(Drone drone, string iconName) {
        // TODO: Use custom component
        var icon = new Sprite();
        icon.Texture = droneIconTextures[iconName];
        icon.Position = LogicToMapPosition(drone.position);
        icon.Scale = new Vector2(1.0f, 1.0f);
        droneIcons.Add(drone.name, icon);
        this.dronesNode.AddChild(icon);
    }

    public void AddBuildiingToMap(Building building, string iconName) {
        // TODO: Use custom component
        var icon = new Sprite();
        icon.Texture = buildingIconTextures[iconName];
        icon.Position = LogicToMapPosition(building.position);
        icon.Scale = new Vector2(1.0f, 1.0f);
        buildingIcons.Add(building.name, icon);
        this.buildingsNode.AddChild(icon);
    }

    private Vector2 LogicToMapPosition(System.Numerics.Vector2 position) {
        return new Vector2(526 * position.X / 100, 392 * position.Y / 100);
    }

}
