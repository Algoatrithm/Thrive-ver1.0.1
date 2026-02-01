using System;
using Godot;

public partial class LightPossesion : Button
{
    public void _on_mouse_entered()
    {
        Label label = new Label
        {
            GlobalPosition = new Vector2(-300, Size.Y / 2),
            Text = "Light Possesion of the selected being.",
        };
        AddChild(label);
        label.Set("theme_override_colors/font_outline_color", true);
        label.Set("theme_override_constants/outline_size", 5);
    }

    public void _on_mouse_exited()
    {
        GetChild(0).QueueFree();
    }
}
