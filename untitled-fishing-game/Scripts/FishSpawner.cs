using Godot;
using System;

public partial class FishSpawner : Node2D
{
	[Export] public PackedScene[] fishScenes;
	[Export] public float minimumSpawnTime = 1f;
	[Export] public float maximumSpawnTime = 3f;

	private RandomNumberGenerator rng = new RandomNumberGenerator();
	private Timer spawnTimer;
	private Camera2D camera;
	
	public override void _Ready()
	{
		camera = GetParent<Camera2D>();
	
		rng.Randomize();

		spawnTimer = new Timer();
		spawnTimer.OneShot = true;
		spawnTimer.Timeout += OnspawnTimeout;
		AddChild(spawnTimer);

		StartNextTimer();
	}

	public void StartNextTimer()
	{
		spawnTimer.WaitTime = rng.RandfRange(minimumSpawnTime, maximumSpawnTime);
		spawnTimer.Start();
	}

	private void OnspawnTimeout()
	{
		SpawnFish();
		StartNextTimer();
	}

	private void SpawnFish()
	{
		if (fishScenes == null)
		{
			return;
		}

		int index = rng.RandiRange(0, fishScenes.Length - 1);
        PackedScene chosenScene = fishScenes[index];

		Fish fish = chosenScene.Instantiate<Fish>();
		GetTree().CurrentScene.AddChild(fish);

		Vector2 viewPortSize = GetViewport().GetVisibleRect().Size;
		Vector2 halfExtents = viewPortSize * camera.Zoom / 2f;

		Vector2 cameraCenter = GlobalPosition;
		Vector2 cameraTopLeft = cameraCenter - halfExtents;
		Vector2 cameraBottomRight = cameraCenter + halfExtents;
		
		float borderoffset =  100f;
		float randomY = rng.RandfRange(cameraTopLeft.Y + 300, cameraBottomRight. Y);
		float spawnX = fish.direction == Vector2.Right ? cameraTopLeft.X - borderoffset : cameraBottomRight.X + borderoffset;

		fish.GlobalPosition = new Vector2(spawnX, randomY);
	}
}
