using System;
using Godot;

public partial class UtilityServer : Node
{
    public static UtilityServer Instance { get; private set; }
    public const string Path_ScreenMessage = "res://game/popups/screen_message.tscn";

    public override void _Ready()
    {
        Instance = this;
    }

    public enum RootChildNodesOrder
    {
        GraphicsServer,
        InputServer,
        UtilityServer,
        SceneDependenciesServer,
        Main,
    }

    public enum OBJECT
    {
        STONE,
    }

    public enum STORAGE
    {
        WOODEN_PLATFORM,
        STONE_PLATFORM,
    }

    public enum StrandType
    {
        vitality,
        anger,
        disgust,
        fear,
        happines,
        sadness,
        end,
    }

    public enum SettingKeys
    {
        AutoSave,
        LockZoom,
        LockPan,
    }

    public void ShowLoadingScreen()
    {
        GD.Print("Loading...");
    }

    public void HideLoadingScreen()
    {
        GD.Print("Loaded!");
    }

    public void SwitchToAreaDomain(Node2D areaDomainToSwitchTo)
    {
        ShowLoadingScreen();
        GetTree()
            .Root.GetChild((int)UtilityServer.RootChildNodesOrder.Main)
            .FindChild("World")
            .RemoveChild(GetTree().Root.GetChild((int)UtilityServer.RootChildNodesOrder.Main).FindChild("World").GetChild(0));
        GetTree().Root.GetChild(1).FindChild("World").AddChild(areaDomainToSwitchTo);
        HideLoadingScreen();
    }

    public void CallMethod(String nodeName, String methodName)
    {
        try
        {
            var nodeTarget = GetTree().Root.GetChild((int)UtilityServer.RootChildNodesOrder.Main).FindChild(nodeName);
            nodeTarget.Call(methodName);
        }
        catch
        {
            GD.PushError(
                $"Something went wrong with CallMethod(String {nodeName}, String {methodName})."
            );
        }
    }

    public void CallMethod(String nodeName, String methodName, Variant arg1)
    {
        try
        {
            var nodeTarget = GetTree().Root.GetChild((int)UtilityServer.RootChildNodesOrder.Main).FindChild(nodeName);
            nodeTarget.Call(methodName, arg1);
        }
        catch
        {
            GD.PushError(
                $"Something went wrong with CallMethod(String {nodeName}, String {methodName})."
            );
        }
    }

    public void CallMethod(String nodeName, String methodName, Variant arg1, Variant arg2)
    {
        try
        {
            var nodeTarget = GetTree().Root.FindChild(nodeName);
            nodeTarget.Call(methodName, arg1, arg2);
        }
        catch
        {
            GD.PushError(
                $"Something went wrong with CallMethod(String {nodeName}, String {methodName})."
            );
        }
    }

    public void CallMethod(
        String nodeName,
        String methodName,
        Variant arg1,
        Variant arg2,
        Variant arg3
    )
    {
        try
        {
            var nodeTarget = GetTree().Root.FindChild(nodeName);
            nodeTarget.Call(methodName, arg1, arg2, arg3);
        }
        catch
        {
            GD.PushError(
                $"Something went wrong with CallMethod(String {nodeName}, String {methodName})."
            );
        }
    }
}
