using System;
using System.Collections.Generic;
using Godot;

public partial class Main : Node2D
{
    SceneTreeTimer WorldTimer;
    Label _AutoSave;

    Godot.Collections.Dictionary<string, Variant> Settings = new Godot.Collections.Dictionary<
        string,
        Variant
    >
    {
        { "AutoSave", false },
    };

    public override void _Ready()
    {
        if ((bool)Settings["AutoSave"])
        {
            _AutoSave = (Label)FindChild("AutoSave");
            WorldTimer = GetTree().CreateTimer(30);
            if (!WorldTimer.IsConnected(SceneTreeTimer.SignalName.Timeout, Callable.From(AutoSave)))
                WorldTimer.Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(AutoSave));
        }
    }

    public override void _Process(double delta)
    {
        if ((bool)Settings["AutoSave"])
            _AutoSave.Text = "Auto Saving in " + ((int)WorldTimer.TimeLeft).ToString();
    }

    private void Save(Node who)
    {
        // Save who
    }

    private void AutoSave()
    {
        WorldServer.Instance.CallMethod("OmnicientControl", "SetWarning", "Saved!");
        WorldTimer = GetTree().CreateTimer(30);
        if (!WorldTimer.IsConnected(SceneTreeTimer.SignalName.Timeout, Callable.From(AutoSave)))
            WorldTimer.Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(AutoSave));
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest)
        {
            GetTree().Quit(); // default behavior
        }
    }
}
