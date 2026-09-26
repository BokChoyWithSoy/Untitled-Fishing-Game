using Godot;
using System;

public partial class Fish : Area2D
{
	[Export] public float speed = 150f;
	public Vector2 direction;
	private AnimatedSprite2D fishSprite;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		fishSprite  = GetNode<AnimatedSprite2D>("FishSprite");
		direction = new Random().Next(0, 2) == 0 ? Vector2.Left : Vector2.Right;
		fishSprite.FlipH = direction == Vector2.Left ? true : false;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += direction * speed * (float)delta;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Hook hook)
		{
			speed = 0;
			fishSprite.FlipH = false;
		}
	}

	private void OnAreaExited(Area2D area)
	{
	}
}
