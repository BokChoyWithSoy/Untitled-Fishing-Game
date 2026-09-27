using Godot;

public partial class CollectionLogPanel : Control
{
    public override void _Ready()
    {
        Visible = false;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("toggle_log"))
        {
            Visible = !Visible;

            if (Visible)
            {
                var ui = GetNode<CollectionLogUI>("%CollectionLogUI");
                ui.Refresh();
            }
        }
    }
}