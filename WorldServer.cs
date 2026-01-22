using System;
using System.Threading.Tasks;
using Godot;

public partial class WorldServer : Node
{
    [Signal]
    public delegate void ObjectPassedEventHandler(
        Node2D obj,
        Godot.Collections.Array<Callable> callables
    );

    [Signal]
    public delegate void ClearObjectPassedEventHandler();

    [Signal]
    public delegate void EventEventHandler();
    public const string Path_ScreenMessage = "res://game/tscn/popups/screen_message.tscn";
    public static WorldServer Instance { get; private set; }

    public enum OBJECT
    {
        STONE,
    }

    public enum STORAGE
    {
        WOODEN_PLATFORM,
        STONE_PLATFORM,
    }

    public override void _Ready()
    {
        Instance = this;
    }

    public void SetObject(Node2D obj)
    {
        EmitSignal(SignalName.ObjectPassed, obj);
    }

    public void SetObject(Node2D obj, Godot.Collections.Array<Callable> arg1)
    {
        EmitSignal(SignalName.ObjectPassed, obj, arg1);
    }

    public void ClearObject()
    {
        EmitSignal(SignalName.ClearObjectPassed);
    }

    // Type Path_ and it should show available paths
    public static Node GetScene(string path)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>(path);
        return scene.Instantiate();
    }
}
