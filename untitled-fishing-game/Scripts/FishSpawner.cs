using Godot;
using System;

public partial class FishSpawner : Node2D
{
	[Export] public PackedScene fishScene;
	[Export] public float minimumSpawnTime = 1f;
	[Export] public float maximumSpawnTime = 3f;

	private RandomNumberGenerator rng = new RandomNumberGenerator();
	private Timer spawnTimer;
	private Camera2D camera;
	
	public override void _Ready()
	{
		camera = GetViewport().GetCamera2D();
		rng.Randomize();

		spawnTimer = new Timer();
		spawnTimer.OneShot = true;
		spawnTimer.Timeout += OnspawnTimeout;
		AddChild(spawnTimer);

		startNextTimer();
	}

	public void startNextTimer()
	{
		spawnTimer.WaitTime = rng.RandfRange(minimumSpawnTime, maximumSpawnTime);
		spawnTimer.Start();
	}

	private void OnspawnTimeout()
	{
		SpawnFish();
		startNextTimer();
	}

	private void SpawnFish()
	{
		if (fishScene == null)
		{
			return;
		}

		Fish fish = fishScene.Instantiate<Fish>();

		GetTree().CurrentScene.AddChild(fish);
		Vector2 screenSize = GetViewport().GetVisibleRect().Size;
		
		float spawnY = rng.RandfRange(0, screenSize.Y);

		float SpawnX = fish.direction == Vector2.Right ? -50 : screenSize.X + 50;

		fish.GlobalPosition = new Vector2(SpawnX, spawnY);
		GD.Print("Spanwed");
	}
}
