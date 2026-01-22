using System;
using Godot;

public partial class Region0 : Node2D
{
    TileMapLayer Map;

    public override void _Ready()
    {
        Map = GetNode<TileMapLayer>("Map");
    }

    public bool IsOnLand(Vector2 pos)
    {
        return Map.GetCellSourceId(Map.LocalToMap(pos)) > -1;
    }
}
