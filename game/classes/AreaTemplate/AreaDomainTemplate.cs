using System;
using Godot;

public partial class AreaDomainTemplate : Node2D
{
    private TileMapLayer MapLand;
    private TileMapLayer MapTree;
    private TileMapLayer MapRock;
    private TileMapLayer MapLandForm;

    Vector2I LandCell = new Vector2I(100, 100);
    Vector2 LandPixel;

    Godot.Collections.Array<Vector2I> LandVariety = new Godot.Collections.Array<Vector2I>
    {
        new Vector2I(0, 0),
        new Vector2I(1, 0),
        new Vector2I(2, 0),
        new Vector2I(3, 0),
        new Vector2I(4, 0),
        new Vector2I(0, 1),
        new Vector2I(1, 1),
        new Vector2I(2, 1),
        new Vector2I(3, 1),
        new Vector2I(4, 1),
        //new Vector2I(0, 4),
        //new Vector2I(2, 4),
        //new Vector2I(4, 4),
    };
    Godot.Collections.Array<Vector2I> LandFormVariety = new Godot.Collections.Array<Vector2I>
    {
        new Vector2I(0, 4),
        new Vector2I(2, 4),
        new Vector2I(4, 4),
    };
    Godot.Collections.Array<Vector2I> TreeVariety = new Godot.Collections.Array<Vector2I>
    {
        new Vector2I(21, 18),
        //new Vector2I(24, 18),
        //new Vector2I(25, 18),
        //new Vector2I(27, 18),
        new Vector2I(28, 18),
        //new Vector2I(25, 20),
        new Vector2I(28, 20),
        new Vector2I(32, 18),
        new Vector2I(33, 18),
        new Vector2I(34, 18),
        new Vector2I(35, 18),
        new Vector2I(37, 18),
        new Vector2I(38, 18),
        new Vector2I(39, 18),
        new Vector2I(32, 19),
        new Vector2I(35, 19),
        new Vector2I(36, 19),
        new Vector2I(37, 20),
    };
    Godot.Collections.Array<Vector2I> RockVariety = new Godot.Collections.Array<Vector2I>
    {
        new Vector2I(6, 21),
        new Vector2I(8, 21),
        new Vector2I(12, 21),
        new Vector2I(14, 21),
        new Vector2I(16, 21),
        new Vector2I(17, 21),
        new Vector2I(18, 21),
        new Vector2I(19, 21),
        new Vector2I(20, 21),
        new Vector2I(12, 23),
        new Vector2I(14, 23),
        new Vector2I(16, 23),
        new Vector2I(17, 23),
        new Vector2I(18, 23),
        new Vector2I(19, 23),
        new Vector2I(20, 23),
        new Vector2I(14, 23),
        new Vector2I(14, 23),
        new Vector2I(6, 24),
        new Vector2I(8, 24),
        new Vector2I(12, 24),
        new Vector2I(14, 24),
        new Vector2I(16, 24),
        new Vector2I(17, 24),
        new Vector2I(18, 24),
        new Vector2I(19, 24),
        new Vector2I(20, 24),
        new Vector2I(12, 26),
        new Vector2I(14, 26),
        new Vector2I(16, 26),
        new Vector2I(17, 26),
        new Vector2I(18, 26),
        new Vector2I(19, 26),
        new Vector2I(20, 26),
    };

    public void GenerateArea(
        Godot.Collections.Dictionary<AreaTemplate._Variables, Variant> var = null
    )
    {
        MapLand = (TileMapLayer)FindChild("Land");
        MapTree = (TileMapLayer)FindChild("Trees");
        MapRock = (TileMapLayer)FindChild("Rocks");

        Vector2I AtlasPos = new Vector2I(0, 0);
        int SourceId = 0;
        LandPixel = MapLand.MapToLocal(LandCell);

        // Draw the Land
        for (int x = 0; x < LandCell.X; x++)
        {
            for (int y = 0; y < LandCell.Y; y++)
            {
                Vector2I NewPos = new Vector2I(x, y);
                MapLand.SetCell(
                    NewPos,
                    SourceId,
                    LandVariety[GD.RandRange(0, LandVariety.Count - 1)]
                );
            }
        }

        // Draw the trees
        NoiseTexture2D texture = new NoiseTexture2D();
        texture.Width = LandCell.X;
        texture.Height = LandCell.Y;
        FastNoiseLite noise = new FastNoiseLite
        {
            NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth,
            Frequency = GD.Randf(),
            Seed = (int)GD.Randi(),
        };
        texture.Noise = noise;
        for (int x = 0; x < LandCell.X; x++)
        {
            for (int y = 0; y < LandCell.Y; y++)
            {
                float point = noise.GetNoise2D(x, y);
                // Tree Generation
                if (point > 0.1f && point < 0.4f)
                {
                    Vector2I NewPos = new Vector2I(x, y);
                    MapTree.SetCell(
                        NewPos,
                        SourceId,
                        TreeVariety[GD.RandRange(0, TreeVariety.Count - 1)]
                    );
                }
                // Rock/Stone Generation
                else if (point >= 0.4f && point < 1.0f)
                {
                    Vector2I NewPos = new Vector2I(x, y);
                    MapRock.SetCell(
                        NewPos,
                        SourceId,
                        RockVariety[GD.RandRange(0, RockVariety.Count - 1)]
                    );
                }
            }
        }

        /* Draw the rocks
        Map = (TileMapLayer)FindChild("Rocks");
        noise = new FastNoiseLite
        {
            NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
            Frequency = GD.Randf(),
            Seed = (int)GD.Randi(),
        };
        texture.Noise = noise;

        for (int x = 0; x < LandCell.X; x++)
        {
            for (int y = 0; y < LandCell.Y; y++)
            {
                if (noise.GetNoise2D(x, y) > 0.6f)
                {
                    Vector2I NewPos = new Vector2I(x, y);
                    Map.SetCell(
                        NewPos,
                        SourceId,
                        RockVariety[GD.RandRange(0, RockVariety.Count - 1)]
                    );
                }
            }
        }*/
    }
}
