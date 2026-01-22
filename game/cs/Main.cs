using System;
using System.Collections.Generic;
using Godot;

public partial class Main : Node2D
{
    public override void _Ready() { }

    public override void _Process(double delta) { }

    private void GenerateStones()
    {
        NoiseTexture2D res = ResourceLoader.Load<NoiseTexture2D>(
            "res://game/world_object/noise generation/StoneGeneration.tres"
        );
    }
}
