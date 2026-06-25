using Godot;
using System;

public partial class Hud : Control
{
	private PackedScene _lifeScene = GD.Load<PackedScene>("res://scenes/ui_life.tscn");

	private Label _score;
	private HBoxContainer _lives;

// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_score = GetNode<Label>("Score");
		_lives = GetNode<HBoxContainer>("Lives");
}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetScore(int score)
	{
		_score.Text = $"$ SCoRE: {score}";
	}

	public void RenderLives(int amount)
	{
		foreach (var live in _lives.GetChildren())
		{
			if (live is TextureRect liveText)
			{
				liveText.QueueFree();
			}
		}

		for (int i = 0; i < amount; i++)
		{
			var life = _lifeScene.Instantiate<TextureRect>();

			_lives.AddChild(life);
		}
	}
}
