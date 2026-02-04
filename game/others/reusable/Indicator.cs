using System;
using Godot;

public partial class Indicator : Label
{
    public async void Display(String text)
    {
        Text = text;
        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        Text = "";
    }
}
