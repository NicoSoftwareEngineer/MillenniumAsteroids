using Godot;
using System;

public partial class TieFighter : Area2D
{
	private bool _isInvulnerable = false;
	private Sprite2D _sprite;
	private CollisionShape2D _cObject;

	private Texture2D _bigTieFighter = GD.Load<Texture2D>("res://assets/tie_fighter.png");
	private Texture2D _smallTieFighter = GD.Load<Texture2D>("res://assets/tie_fighter_exploded.png");

	private Shape2D _bigCShape = GD.Load<Shape2D>("res://collision_shapes/big_tie_fighter.tres");
	private Shape2D _smallCShape = GD.Load<Shape2D>("res://collision_shapes/small_tie_fighter.tres");

	[Signal]
	public delegate void ExplodedEventHandler(TieFighter size);

	[Export]
	public int Speed { get; set; } = 75;

	public int PointsPerDestruction => TieFighterSize == Size.Big ? 100 : 50;

	private Vector2 Vector { get; set; } = new Vector2(1f, 0f);

	[Export] public Size TieFighterSize = Size.Big;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_cObject = GetNode<CollisionShape2D>(" CollisionShape2D");

		switch (TieFighterSize)
		{
			case Size.Big:
				Speed = GD.RandRange(50, 100);
				_sprite.Texture = _bigTieFighter;
				_cObject.Shape = _bigCShape;
				break;
			case Size.Small:
				Speed = GD.RandRange(100, 150);
				_sprite.Texture = _smallTieFighter;
				_cObject.Shape = _smallCShape;
				break;
			default:
				return;
		}
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

	public void Explode()
	{
		EmitSignal(SignalName.Exploded, this);
		QueueFree();
	}

	private void OnBodyEntered(Node2D node)
	{
		if (node is Millennium millennium)
		{
			if (_isInvulnerable) return;

			millennium.Die();

			GD.Print("Player should be killed by ship");

			TriggerInvulnerability();
		}
	}

	private async void TriggerInvulnerability()
	{
		_isInvulnerable = true;

		await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);

		_isInvulnerable = false;
	}

	private void ScreenWrap()
	{
		Vector2 screenSize = GetViewportRect().Size;

		if (GlobalPosition.X < 0)
		{
			GlobalPosition = GlobalPosition with { X = screenSize.X };
		}
		else if (GlobalPosition.X > screenSize.X)
		{
			GlobalPosition = GlobalPosition with { X = 0f };
		}

		if (GlobalPosition.Y < 0) GlobalPosition = GlobalPosition with { Y = screenSize.Y };
		else if (GlobalPosition.Y > screenSize.Y) GlobalPosition = GlobalPosition with { Y = 0f };
	}

	public enum Size
	{
		Big,
		Small
	}
}
