using System;
using Godot;

public partial class AreaDomainTemplate : Node2D
{
    private TileMapLayer Map;

    public void GenerateArea(
        Godot.Collections.Dictionary<AreaTemplate._Variables, Variant> var = null
    )
    {
        Vector2I Pos = new Vector2I(100, 100);
        Vector2I AtlasPos = new Vector2I(0, 0);

        int SourceId = 0;

        Map = (TileMapLayer)GetChild(0);

        for (int x = 0; x < Pos.X; x++)
        {
            for (int y = 0; y < Pos.Y; y++)
            {
                Vector2I NewPos = new Vector2I(x, y);
                Map.SetCell(NewPos, SourceId, AtlasPos);
            }
        }
    }
}
