using Godot;
using System;

public partial class Game : Node2D
{
	private Node2D _lasers;
	private Node2D _tieFighters;
	private Millennium _millennium;
	
	private PackedScene _tieFighterScene = GD.Load<PackedScene>("res://scenes/tie_fighter.tscn");
	
	public override void _Ready()
	{
		_lasers = GetNode<Node2D>("Lasers");
		_tieFighters = GetNode<Node2D>("TieFighters");
		_millennium = GetNode<Millennium>("Millennium");
		
		_millennium.LaserShot += OnLaserShot;
		
		for(int i = 0; i < 3; i++){
			CreateNewTieFighter();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("reset")){
			GetTree().ReloadCurrentScene();
		}
	}
	
	private void OnLaserShot(Area2D laser)
	{
		_lasers.AddChild(laser);
	}
	
	private void OnTieFighterExploded(){
		var tieFightersToRegenerate = GD.RandRange(1, 2);
		for(int i = 0; i < tieFightersToRegenerate; i++){
			 CreateNewTieFighter();
		}
	}
	
	private void CreateNewTieFighter()
	{
		Vector2 screenSize = GetViewportRect().Size;
		var tieFighter = _tieFighterScene.Instantiate<TieFighter>();
		
		var side = GD.Randi() % 3;
		var rndX =  GD.Randi() % screenSize.X;
		var rndY =  GD.Randi() % screenSize.Y;
		
		switch(side)
		{
			case 1: // Right side
				tieFighter.Rotation = GD.RandRange(125, 240);
				rndX = screenSize.X;
				break;
			case 2: // Bottom side
				tieFighter.Rotation = GD.RandRange(-150, -35);
				rndY = screenSize.Y;
				break;
			case 3: // Left side
				tieFighter.Rotation = GD.RandRange(-125, -240);
				rndX = screenSize.X;
				break;
			default: // Top side
				tieFighter.Rotation = GD.RandRange(150, 35);
				rndY = screenSize.Y;
				break;
		}
		
		tieFighter.GlobalPosition = new Vector2(rndX,rndY);
		tieFighter.Exploded += OnTieFighterExploded;
		_tieFighters.AddChild(tieFighter);
	}
}
