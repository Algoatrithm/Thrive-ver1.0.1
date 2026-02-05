using System;
using Godot;

public partial class GraphicsServer : Node
{
    public static GraphicsServer Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }
}
