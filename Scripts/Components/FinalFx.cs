using Godot;

public class FinalFx: Control {
	private float t;
	public override void _Process(float delta) {
		t += delta;
		this.Modulate = new Color(1, 1, 1, Mathf.Sin(2 * t));
	}
}
