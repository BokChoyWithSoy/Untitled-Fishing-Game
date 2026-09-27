using Godot;
using System.Collections.Generic;

public partial class CollectionLog : Node
{
    public HashSet<string> caughtFishIds = new HashSet<string>();

    public void RecordCatch(string fishId)
    {
        caughtFishIds.Add(fishId);
    }

    public bool HasCaught(string fishId) => caughtFishIds.Contains(fishId);
}