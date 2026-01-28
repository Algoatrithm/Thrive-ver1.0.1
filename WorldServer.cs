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

    public override void _Ready()
    {
        Instance = this;
    }

    public void CallMethod(String nodeName, String methodName, Variant arg1)
    {
        try
        {
            var nodeTarget = GetTree().Root.GetChild(1).FindChild(nodeName, false);
            nodeTarget.Call(methodName, arg1);
        }
        catch
        {
            GD.PushError("Something went wrong with CallMethod function with 1 parameter");
        }
    }

    public GpuParticles2D CreateStrand(StrandType strandType, Node2D node)
    {
        GpuParticles2D strand = new()
        {
            ZIndex = 10,
            Amount = 5,
            Texture = ResourceLoader.Load<Texture2D>("res://game/res/single_smokelete.png"),
            Lifetime = 3.0f,
            Randomness = 1.0f,
            LocalCoords = true,
            TrailEnabled = true,
            TrailLifetime = 2.0f,
            TrailSections = 16,
            TrailSectionSubdivisions = 16,
            ProcessMaterial = ResourceLoader.Load<ParticleProcessMaterial>(
                "res://game/res/" + strandType.ToString() + ".tres"
            ),
        };

        node.AddChild(strand);
        return strand;
    }

    public void CallMethod(String nodeName, String methodName, Variant arg1, Variant arg2)
    {
        try
        {
            var nodeTarget = GetTree().Root.FindChild(nodeName, false);
            nodeTarget.Call(methodName, arg1, arg2);
        }
        catch
        {
            GD.PushError("Something went wrong with CallMethod function with 2 parameters");
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
            var nodeTarget = GetTree().Root.FindChild(nodeName, false);
            nodeTarget.Call(methodName, arg1, arg2, arg3);
        }
        catch
        {
            GD.PushError("Something went wrong with CallMethod function with 3 parameters");
        }
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
