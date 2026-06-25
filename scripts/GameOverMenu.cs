using Godot;

public partial class GameOverMenu : Control
{

	[Signal]
	public delegate void RestartGameEventHandler();
// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnRetryButtonPressed()
	{
		EmitSignal("RestartGame");
		Visible = false;
	}
}
