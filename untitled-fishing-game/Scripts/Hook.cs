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
	private CollectionLog collectionLog;

	public override void _Ready() 
	{
		//setup hooks
		AreaEntered += OnAreaEntered;
		AreaExited += OnAreaExited;

		//setup collection log
		collectionLog = GetNode<CollectionLog>("%CollectionLog");

		//setup player animator
		playerSprite = GetNode<AnimatedSprite2D>("%PlayerSprite");

		startingPosition = GlobalPosition;

        //Setup the fishing line and add it the hook as a child
        ropeLine = new Line2D
        {
            Width = 1f,
            DefaultColor = Colors.Black,
            TopLevel = true,
            ZIndex = 4,
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
		Input.MouseMode = Input.MouseModeEnum.Confined;

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
		if (area is Water)
		{
			isInWater = true;
		}

		if (area is Fish fish)
		{
			bool canCatch = (caughtFish == null && fish.size == 1) || (caughtFish != null && fish.size == caughtFish.size + 1);
			if (!canCatch) return;

			caughtFish?.QueueFree();

			caughtFish = fish;
			playerSprite.Play("Reel");
			fish.Reparent(this);
			fish.SetPhysicsProcess(false);
			fish.Position = fish.HookedPosition;
			fish.Rotation = fish.direction == Vector2.Left ? Mathf.Pi / 2f : -Mathf.Pi / 2f;
			fish.PauseDespawnTimer();
		}
	}

	private void OnAreaExited(Area2D area) 
	{
		if (area is Water)
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
			collectionLog.RecordCatch(caughtFish.fishId);

			caughtFish.QueueFree();
			caughtFish = null;

			playerSprite.Play("Idle");
		}
	}
}
