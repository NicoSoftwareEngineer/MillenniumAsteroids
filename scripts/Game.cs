using Godot;
using System;

public partial class Game : Node2D
{
	private Node2D _lasers;
	private Node2D _tieFighters;
	private Node2D _respawnPoint;
	private Millennium _millennium;
	private GameOverMenu _gameOverMenu;
	private StartMenu _startGameMenu;
	private Hud _hud;
	private bool _isInvulnerable = false;
	private bool _hasGameStarted = false;

	private PackedScene _tieFighterScene = GD.Load<PackedScene>("res://scenes/tie_fighter.tscn");

	private int _score = 0;
	public int Score
	{
		get => _score;
		set
		{
			_score = value;
			_hud.SetScore(_score);
		}
	}

	private int _lives = 0;
	public int Lives
	{
		get => _lives;
		set
		{
			_lives = value;
			_hud?.RenderLives(Lives);
		}
	}

	public override void _Ready()
	{
		_lasers = GetNode<Node2D>("Lasers");
		_tieFighters = GetNode<Node2D>("TieFighters");
		_respawnPoint = GetNode<Node2D>("RespawnPoint");
		_millennium = GetNode<Millennium>("Millennium");
		_hud = GetNode<Hud>("UI/HUD");
		_gameOverMenu = GetNode<GameOverMenu>("UI/GameOverMenu");
		_startGameMenu = GetNode<StartMenu>("UI/StartMenu");

		_gameOverMenu.Visible = false;
		_millennium.Visible = false;

		_startGameMenu.StartGame += StartGame;
		_gameOverMenu.RestartGame += RestartGame;

		_millennium.LaserShot += OnLaserShot;
		_millennium.Died += OnPlayerDied;
		_millennium.Die();

		Vector2 screenSize = GetViewportRect().Size;
		_respawnPoint.GlobalPosition = new Vector2(screenSize.X / 2, screenSize.Y / 2);

		_millennium.GlobalPosition = new Vector2(screenSize.X / 2, screenSize.Y / 2);

		Lives = 3;
		Score = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("reset"))
		{
			GetTree().ReloadCurrentScene();
		}

		if (GD.RandRange(0, 300) == 1 && _hasGameStarted)
		{
			CreateNewTieFighter();
		}
	}

	private void OnLaserShot(Area2D laser)
	{
		_lasers.AddChild(laser);
	}

	private void OnTieFighterExploded(TieFighter fighter)
	{
		Score += fighter.PointsPerDestruction;

		if (fighter.TieFighterSize == TieFighter.Size.Big)
		{
			CreateBrokenTieFighter(fighter.GlobalPosition);
		}
		else
		{
			if (GD.RandRange(0, 1) == 1)
			{
				CreateNewTieFighter();
			}
		}
	}

	private void CreateBrokenTieFighter(Vector2 position)
	{
		var newRotation = GD.RandRange(0, 359);

		for (int i = 0; i < 2; i++)
		{
			var tieFighter = _tieFighterScene.Instantiate<TieFighter>();
			tieFighter.Rotation = (int)Math.Round(Math.Pow(-1, i)) * newRotation;
			tieFighter.TieFighterSize = TieFighter.Size.Small;
			tieFighter.GlobalPosition = position;
			tieFighter.Exploded += OnTieFighterExploded;
			_tieFighters.AddChild(tieFighter);
		}
	}

	private void CreateNewTieFighter()
	{
		Vector2 screenSize = GetViewportRect().Size;
		var tieFighter = _tieFighterScene.Instantiate<TieFighter>();

		var side = GD.Randi() % 3;
		var rndX = GD.Randi() % screenSize.X;
		var rndY = GD.Randi() % screenSize.Y;

		switch (side)
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

		tieFighter.TieFighterSize = TieFighter.Size.Big;
		tieFighter.GlobalPosition = new Vector2(rndX, rndY);
		tieFighter.Exploded += OnTieFighterExploded;
		_tieFighters.AddChild(tieFighter);
	}

	public void OnPlayerDied()
	{
		if (!_hasGameStarted)
		{
			return;
		}
		DecreaseHealth();
	}

	private void DecreaseHealth()
	{
		Lives--;

		if (Lives <= 0)
		{
			_gameOverMenu.Visible = true;
		}
		else
		{
			_millennium.Respawn(_respawnPoint.GlobalPosition);
		}
	}

	private void StartGame()
	{
		_millennium.Respawn(_respawnPoint.GlobalPosition);
		_startGameMenu.Visible = false;

		Lives = 3;
		Score = 0;
		_hasGameStarted = true;

		for (int i = 0; i < 3; i++)
		{
			CreateNewTieFighter();
		}
	}

	public void RestartGame()
	{
		foreach (var tieFighterToBeCleared in _tieFighters.GetChildren())
		{
			tieFighterToBeCleared.QueueFree();
		}
		_millennium.Respawn(_respawnPoint.GlobalPosition);
		StartGame();
	}
}
