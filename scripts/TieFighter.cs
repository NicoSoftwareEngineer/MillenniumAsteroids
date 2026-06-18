using Godot;
using System;

public partial class TieFighter : Area2D
{
	[Signal]
	public delegate void ExplodedEventHandler();
	
	[Export]
	public int Speed { get; set; } = 200;
	
	private Vector2 Vector { get; set; } = new Vector2(1f,0f);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition += Vector.Rotated(Rotation) * Speed * (float)delta;
		
		ScreenWrap();
	}
	
	public void Explode(){
		EmitSignal(SignalName.Exploded);
		QueueFree();
	}
	
	private void ScreenWrap()
	{
		Vector2 screenSize = GetViewportRect().Size;

		if (GlobalPosition.X < 0){
			 GlobalPosition = GlobalPosition with {X = screenSize.X };
		}
		else if (GlobalPosition.X > screenSize.X){
			 GlobalPosition = GlobalPosition with {X = 0f };
		}

		if (GlobalPosition.Y < 0) GlobalPosition = GlobalPosition with {Y = screenSize.Y };
		else if (GlobalPosition.Y > screenSize.Y)  GlobalPosition = GlobalPosition with {Y = 0f };
	}
}
