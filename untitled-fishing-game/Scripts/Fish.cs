using Godot;
using System;

public partial class Fish : Area2D
{
	[Export] public float speed = 150f;
	[Export] public int size = 1;
	[Export] public Vector2 HookedPosition = new Vector2(0f, 15f);
	[Export] public string fishId;
	public Vector2 direction;
	private AnimatedSprite2D fishSprite;
	private Timer despawnTimer;

	public override void _Ready()
	{
		fishSprite  = GetNode<AnimatedSprite2D>("FishSprite");
		direction = new Random().Next(0, 2) == 0 ? Vector2.Left : Vector2.Right;
		fishSprite.FlipH = direction == Vector2.Left ? true : false;

        despawnTimer = new Timer
        {
            WaitTime = 20f,
            OneShot = true
        };
        despawnTimer.Timeout += () => QueueFree();
        AddChild(despawnTimer);
        despawnTimer.Start();
	}	

	public override void _PhysicsProcess(double delta)
	{
		Position += direction * speed * (float)delta;
	}

	public void PauseDespawnTimer()
	{
		despawnTimer.Paused = true;
	}
}
