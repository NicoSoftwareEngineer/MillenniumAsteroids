using Godot;
using System;

public partial class Laser : Area2D
{
	[Export]
	public int Speed { get; set; } = 1000;
	
	private Vector2 Vector { get; set; } = new Vector2(1f,0f);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition += Vector.Rotated(Rotation) * Speed * (float)delta;
	}
	
	public void LaserExitedScreen()
	{
		QueueFree();
	}
}
