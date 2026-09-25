using Godot;
using System;

public partial class Hook : Area2D
{
	private float startingYPosition;
	private Line2D ropeLine;

	public override void _Ready() {
		startingYPosition = GlobalPosition.Y;

		//Setup the fishing line and add it the hook as a child
		ropeLine = new Line2D();
		ropeLine.Width = 0.5f;
		ropeLine.DefaultColor = Colors.Black;
		ropeLine.TopLevel = true;
		AddChild(ropeLine);
	}

	public override void _PhysicsProcess(double delta) {
		//Fish hook calculations
		float globalMousePositionY = GetGlobalMousePosition().Y;
		float targetPositionY = Mathf.Max(startingYPosition, globalMousePositionY);

		GlobalPosition = new Vector2I((int)GlobalPosition.X, (int)targetPositionY);

		Vector2I localStartPoint = new Vector2I(0, (int)(startingYPosition - GlobalPosition.Y));
		Vector2I localEndPoint = Vector2I.Zero;

		ropeLine.ClearPoints();
		ropeLine.AddPoint(localStartPoint);
		ropeLine.AddPoint(localEndPoint);
	}
}
