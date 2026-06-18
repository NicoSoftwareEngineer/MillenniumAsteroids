using Godot;
using System;

public partial class Millennium : CharacterBody2D
{
	[Signal]
	public delegate void LaserShotEventHandler(Area2D laser);
	
	 [Export]
	public int Speed { get; set; } = 1000;
	
	[Export]
	public int Acceleration { get; set; } = 900;
	
	[Export] 
	public float Friction = 10f;

	[Export]
	public float RotationSpeed { get; set; } = 5f;
	
	private Node2D _muzzle;
	
	private PackedScene _laserScene = GD.Load<PackedScene>("res://scenes/laser.tscn");
	
	private bool _breakShots = false;

	private AnimatedSprite2D _animatedSprite;
	
		public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		_muzzle = GetNode<Node2D>("Muzzle");
	}
	
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("shoot"))
		{
			if(!_breakShots){
				ShootLaser();
				_breakShots = true;
			}
		}
		else
		{
			_breakShots = false;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
	
		if (Input.IsActionPressed("steer_right"))
		{
			Rotation += RotationSpeed * (float)delta;
		}

		if (Input.IsActionPressed("steer_left"))
		{
			Rotation -= RotationSpeed * (float)delta;
		}
	
		if (Input.IsActionPressed("thrust"))
		{
			_animatedSprite.Play("thrust");
			Velocity += Transform.X * Acceleration * (float)delta;
			
			Velocity = Velocity.LimitLength(Speed);
		}
		else{
			_animatedSprite.Play("default");
			
			Velocity = Velocity.MoveToward(Vector2.Zero, Friction);
		}

		MoveAndSlide();
		
		ScreenWrap();
	}
	
	private void ScreenWrap()
	{
		Vector2 screenSize = GetViewportRect().Size;
		Vector2 pos = GlobalPosition;

		if (pos.X < 0) pos.X = screenSize.X;
		else if (pos.X > screenSize.X) pos.X = 0;

		if (pos.Y < 0) pos.Y = screenSize.Y;
		else if (pos.Y > screenSize.Y) pos.Y = 0;

		GlobalPosition = pos;
	}
	
	private void ShootLaser()
	{
		var l = _laserScene.Instantiate<Area2D>();
		l.GlobalPosition = _muzzle.GlobalPosition;
		l.Rotation = Rotation;
		EmitSignal(SignalName.LaserShot, l);
	}
}
