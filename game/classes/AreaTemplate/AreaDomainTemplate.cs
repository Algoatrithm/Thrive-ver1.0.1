using System;
using Godot;

public partial class AreaDomainTemplate : Node2D
{
    [Export]
    public bool HasCoastLine = false;

    [Export]
    public bool HasLake = false;

    [Export]
    public bool HasRiver = false;
    private TileMapLayer Map;

    public enum _Variables
    {
        HasCoastLine,
        HasLake,
        HasRiver,
        None,
    }

    public void GenerateArea(Godot.Collections.Array<_Variables> var = null)
    {
        HasCoastLine = false;
        HasLake = false;
        HasRiver = false;

        if (var != null)
        {
            foreach (_Variables _var in var)
            {
                switch (_var)
                {
                    case _Variables.HasCoastLine:
                        HasCoastLine = true;
                        break;
                    case _Variables.HasLake:
                        HasLake = true;
                        break;
                    case _Variables.HasRiver:
                        HasRiver = true;
                        break;
                }
            }
        }

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
