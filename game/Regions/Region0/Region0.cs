using System;
using Godot;

public partial class Region0 : Node2D
{
    TileMapLayer Map;

    public override void _Ready()
    {
        Map = (TileMapLayer)FindChild("Land");
    }

    public bool IsOnLand(Vector2 pos)
    {
        return Map.GetCellSourceId(Map.LocalToMap(pos)) > -1;
    }
}
