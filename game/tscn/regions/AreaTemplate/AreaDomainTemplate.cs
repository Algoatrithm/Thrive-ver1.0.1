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

        int SizeX = 350;
        int SizeY = 250;

        int AtlasPosX = 0;
        int AtlasPosY = 0;

        int SourceId = 0;

        Map = (TileMapLayer)GetChild(0);

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                Vector2I pos = new Vector2I(SizeX, SizeY);
                Map.SetCell(pos, SourceId, new Vector2I(AtlasPosX, AtlasPosY));
            }
        }
    }
}
