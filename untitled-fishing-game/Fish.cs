using Godot;
using System;

public partial class Fish : Area2D
{
	public float speed = 150f;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	public override void _Process(double delta)
	{
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Hook hook)
		{
			GD.Print("Fish caught!");
		}
	}
}
