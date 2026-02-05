using System;
using System.Collections.Generic;
using Godot;

public partial class Main : Node2D
{
    SceneTreeTimer WorldTimer;
    Label _AutoSave;

    public override void _Ready()
    {
        Engine.MaxFps = 120;
        if ((bool)SceneDependenciesServer.Instance.GetSetting(UtilityServer.SettingKeys.AutoSave))
        {
            _AutoSave = (Label)FindChild("AutoSave");
            WorldTimer = GetTree().CreateTimer(30);
            if (!WorldTimer.IsConnected(SceneTreeTimer.SignalName.Timeout, Callable.From(AutoSave)))
                WorldTimer.Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(AutoSave));
        }
    }

    public override void _Process(double delta)
    {
        GD.Print(Engine.GetFramesPerSecond());
        if ((bool)SceneDependenciesServer.Instance.GetSetting(UtilityServer.SettingKeys.AutoSave))
            _AutoSave.Text = "Auto Saving in " + ((int)WorldTimer.TimeLeft).ToString();
    }

    private void Save(Node who)
    {
        // Save who
    }

    private void AutoSave()
    {
        UtilityServer.Instance.CallMethod("OmnicientControl", "SetWarning", "Saved!");
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
