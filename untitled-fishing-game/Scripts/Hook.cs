using Godot;
using System;
using System.Collections.ObjectModel;

public partial class Hook : Area2D
{
	private bool isInWater;
	public int score;
	private Vector2 startingPosition;
	private Line2D ropeLine;
	private AnimatedSprite2D playerSprite;
	private Fish caughtFish;

	public override void _Ready() 
	{
		//setup hooks
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		//setup player animator
		playerSprite = GetNode<AnimatedSprite2D>("%PlayerSprite");

		startingPosition = GlobalPosition;

        //Setup the fishing line and add it the hook as a child
        ropeLine = new Line2D
        {
            Width = 1f,
            DefaultColor = Colors.Black,
            TopLevel = true,
            ZIndex = 1,
            ZAsRelative = false
        };
        AddChild(ropeLine);

		isInWater = false;
		score = 0;
	}

	public override void _PhysicsProcess(double delta) 
	{
		//Fish hook calculations
		float globalMousePositionY = GetGlobalMousePosition().Y;
		float targetPositionY = Mathf.Max(startingPosition.Y, globalMousePositionY);

		GlobalPosition = new Vector2(startingPosition.X, targetPositionY);

		ropeLine.ClearPoints();
		ropeLine.AddPoint(startingPosition);
		ropeLine.AddPoint(GlobalPosition);

		if (isInWater == false && caughtFish != null)
		{
			playerSprite.Play("Caught");
		}

	}

	private void OnAreaEntered(Area2D area) 
	{
		if (area is Water water)
		{
			isInWater = true;
		}

		if (caughtFish == null)
		{
			if (area is Fish fish)
			{
				caughtFish = fish;
				playerSprite.Play("Reel");
				fish.Reparent(this);
				fish.SetPhysicsProcess(false);
				fish.Position = new Vector2(0,20f);
				fish.Rotation = -Mathf.Pi / 2f;
			}
		}
	}

	private void OnAreaExited(Area2D area) 
	{
		if (area is Water water)
		{
			isInWater = false;
		}
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed && mouseButton.ButtonIndex == MouseButton.Left)
		{
			CollectFish();
		}
    }

	private void CollectFish()
	{
		if (isInWater == false && caughtFish != null)
		{
			score++;
			GD.Print(score);

			caughtFish.QueueFree();
			caughtFish = null;

			playerSprite.Play("Idle");
		}
	}
}
