using Godot;

public class MusicWindow: BaseWindow {
	private AudioStreamMP3[] songs = new AudioStreamMP3[] {
		ResourceLoader.Load<AudioStreamMP3>("res://Resources/Music/a-hero-of-the-80s-126684.mp3"),
		ResourceLoader.Load<AudioStreamMP3>("res://Resources/Music/cheating-15095.mp3"),
		ResourceLoader.Load<AudioStreamMP3>("res://Resources/Music/chill-lofi-hip-hop-132371.mp3"),
		ResourceLoader.Load<AudioStreamMP3>("res://Resources/Music/chill-lofi-song-8444.mp3"),
		ResourceLoader.Load<AudioStreamMP3>("res://Resources/Music/coffee-chill-out-15283.mp3"),
		ResourceLoader.Load<AudioStreamMP3>("res://Resources/Music/elevator-music-bossa-nova-background-music-version-60s-10900.mp3"),
		ResourceLoader.Load<AudioStreamMP3>("res://Resources/Music/energetic-upbeat-stylish-pop-fashion-136514.mp3"),
	};
	private int playingIndex;
	private AudioStreamPlayer playerNode;
	private Control playNode;
	private Control nextNode;
	private bool hoveringPlay;
	private bool hoveringNext;
	private bool play;
	public override void _Ready() {
		this.playerNode = GetNode<AudioStreamPlayer>("StreamPlayer");
		this.playerNode.Connect("finished", this, nameof(NextSong));

		this.playNode = GetNode<Control>("PlayButton");
		this.nextNode = GetNode<Control>("NextButton");
		this.playNode.Connect("mouse_entered", this, nameof(HandlePlayEntered));
		this.playNode.Connect("mouse_exited", this, nameof(HandlePlayExited));
		this.nextNode.Connect("mouse_entered", this, nameof(HandleNextEntered));
		this.nextNode.Connect("mouse_exited", this, nameof(HandleNextExited));
		this.playNode.Connect("gui_input", this, nameof(HandlePlay));
		this.nextNode.Connect("gui_input", this, nameof(HandleNext));
	}

	private void HandlePlayEntered() {
		this.hoveringPlay = true;
	}
	private void HandlePlayExited() {
		this.hoveringPlay = false;
	}

	private void HandleNextEntered() {
		this.hoveringNext = true;
	}
	private void HandleNextExited() {
		this.hoveringNext = false;
	}

	private void HandlePlay(InputEvent inputEvent) {
		if (inputEvent == null) { return; }
		if (inputEvent is InputEventMouseButton buttonEvent) {
			if (buttonEvent.IsActionPressed("mouse_left") && this.hoveringPlay) {
				if (this.playerNode.Playing) {
					this.playerNode.Stream = null;
					this.playerNode.Stop();
				} else {
					this.playerNode.Stream = this.songs[this.playingIndex % songs.Length];
					this.playerNode.Play();
				}
				this.play = !this.play;
			}
		}
	}
	private void HandleNext(InputEvent inputEvent) {
		if (inputEvent == null) { return; }
		if (inputEvent is InputEventMouseButton buttonEvent) {
			if (buttonEvent.IsActionPressed("mouse_left") && this.hoveringNext) {
				NextSong();
			}
		}
	}

	public override void _Process(float delta) {
		if (this.hoveringPlay) {
			this.playNode.Modulate = new Color("ff9999");
		} else {
			this.playNode.Modulate = new Color("ffffff");
		}
		if (this.hoveringNext) {
			this.nextNode.Modulate = new Color("ff9999");
		} else {
			this.nextNode.Modulate = new Color("ffffff");
		}
	}

	private void NextSong() {
		if (this.play) {
			this.playingIndex++;
			this.playerNode.Stream = this.songs[this.playingIndex % songs.Length];
			this.playerNode.Play();
		}
	}
}
