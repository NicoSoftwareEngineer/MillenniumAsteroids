using Godot;
using System;

public partial class Game : Node2D
{
	private Node2D _lasers;
	private Millennium _millennium;
	
	public override void _Ready()
	{
		_lasers = GetNode<Node2D>("Lasers");
		_millennium = GetNode<Millennium>("Millennium");
		
		_millennium.LaserShot += OnLaserShot;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnLaserShot(Area2D laser)
	{
		_lasers.AddChild(laser);
	}
}
