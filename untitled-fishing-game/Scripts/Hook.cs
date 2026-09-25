using Godot;
using System;

public partial class Hook : Area2D
{
	private Vector2 startingPosition;
	private Line2D ropeLine;

	public override void _Ready() {
		startingPosition = GlobalPosition;

		//Setup the fishing line and add it the hook as a child
		ropeLine = new Line2D();
		ropeLine.Width = 1f;
		ropeLine.DefaultColor = Colors.Black;
		ropeLine.TopLevel = true;
		ropeLine.ZIndex = 1;
		ropeLine.ZAsRelative = false;
		AddChild(ropeLine);
	}

	public override void _PhysicsProcess(double delta) {
		//Fish hook calculations
		float globalMousePositionY = GetGlobalMousePosition().Y;
		float targetPositionY = Mathf.Max(startingPosition.Y, globalMousePositionY);

		GlobalPosition = new Vector2(startingPosition.X, targetPositionY);

		ropeLine.ClearPoints();
		ropeLine.AddPoint(startingPosition);
		ropeLine.AddPoint(GlobalPosition);
	}
}
