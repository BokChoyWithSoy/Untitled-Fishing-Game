using Godot;
using System.Collections.Generic;

public partial class CollectionLogUI : GridContainer
{
    [Export] public PackedScene[] AllFish;
    private CollectionLog collectionLog;

    public override void _Ready()
    {
        collectionLog = GetNode<CollectionLog>("%CollectionLog");
        Refresh();
    }

    public void Refresh()
    {
        foreach (Node child in GetChildren())
            child.QueueFree();

        foreach (PackedScene fishScene in AllFish)
        {
            Fish fishInstance = fishScene.Instantiate<Fish>();

            var panel = new PanelContainer
            {
                CustomMinimumSize = new Vector2(72, 72) // slightly bigger than the icon for padding
            };

            var styleBox = new StyleBoxFlat
            {
                BgColor = new Color("#edd6b3")
            };
            panel.AddThemeStyleboxOverride("panel", styleBox);

            var icon = new TextureRect
            {
                Texture = GetFishIcon(fishInstance),
                CustomMinimumSize = new Vector2(80, 80),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered
            };

            bool caught = collectionLog.HasCaught(fishInstance.fishId);
            icon.Modulate = caught ? Colors.White : Colors.Black;

            panel.AddChild(icon);
            AddChild(panel);

            fishInstance.QueueFree();
        }
    }

    private Texture2D GetFishIcon(Fish fish)
    {
        var sprite = fish.GetNode<AnimatedSprite2D>("FishSprite");
        string anim = sprite.Animation;
        return sprite.SpriteFrames.GetFrameTexture(anim, 0);
    }
}