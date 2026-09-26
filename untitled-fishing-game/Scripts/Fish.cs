using Godot;
using System;

public partial class Fish : Area2D
{
	[Export] public float speed = 150f;
	public Vector2 direction;
	private AnimatedSprite2D fishSprite;

	public override void _Ready()
	{
		fishSprite  = GetNode<AnimatedSprite2D>("FishSprite");
		direction = new Random().Next(0, 2) == 0 ? Vector2.Left : Vector2.Right;
		fishSprite.FlipH = direction == Vector2.Left ? true : false;

		GetTree().CreateTimer(15f).Timeout += () => QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += direction * speed * (float)delta;
	}
}
