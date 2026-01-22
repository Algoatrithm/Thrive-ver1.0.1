using System;
using Godot;

public partial class OmnicientsView : Camera2D
{
    [Export]
    public float Sensitivity = 0.1f;

    public override void _Input(InputEvent @event)
    {
        CameraPan(@event);
        CameraZoom(@event);
    }

    private void CameraPan(InputEvent @event)
    {
        if (Input.IsActionPressed("Click") && @event is InputEventMouseMotion mouseMotion)
            GlobalPosition += mouseMotion.Relative * (Sensitivity * -1);
    }

    private void CameraZoom(InputEvent @event)
    {
        // Zoom in
        if (Input.IsActionPressed("up_Zoom") && Zoom < new Vector2(30, 30))
            Zoom += new Vector2(1, 1);
        // Zoom out
        if (Input.IsActionPressed("down_Zoom") && Zoom > new Vector2(1, 1))
            Zoom -= new Vector2(1, 1);
    }
}
