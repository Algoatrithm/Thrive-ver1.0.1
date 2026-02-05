using System;
using Godot;

public partial class SceneDependenciesServer : Node
{
    public static SceneDependenciesServer Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    [Signal]
    public delegate void ObjectPassedEventHandler(
        Node2D obj,
        Godot.Collections.Array<Callable> callables
    );

    [Signal]
    public delegate void ClearObjectPassedEventHandler();

    [Signal]
    public delegate void EventEventHandler();
    private Node2D CurrentRegion = null;
    private Godot.Collections.Dictionary<string, Variant> Settings =
        new Godot.Collections.Dictionary<string, Variant>
        {
            { "AutoSave", false },
            { "LockZoom", false },
            { "LockPan", false },
        };

    public Node2D GetCurrentRegion()
    {
        return CurrentRegion;
    }

    public void SetCurrentRegion(Node2D region)
    {
        CurrentRegion = region;
    }

    public void SetSetting(UtilityServer.SettingKeys key, Variant val)
    {
        Settings[key.ToString()] = val;
    }

    public Variant GetSetting(UtilityServer.SettingKeys key)
    {
        return Settings[key.ToString()];
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
    } // Type Path_ and it should show available paths

    public static Node GetScene(string path)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>(path);
        return scene.Instantiate();
    }

    public GpuParticles2D CreateStrand(UtilityServer.StrandType strandType, Node2D node)
    {
        GpuParticles2D strand = new()
        {
            ZIndex = 10,
            Amount = 1,
            Texture = ResourceLoader.Load<Texture2D>("res://game/assets/single_smokelete.png"),
            Lifetime = 3.0f,
            Randomness = 1.0f,
            LocalCoords = true,
            TrailEnabled = true,
            TrailLifetime = 2.0f,
            TrailSections = 16,
            TrailSectionSubdivisions = 16,
            Emitting = false,
            OneShot = true,
            ProcessMaterial = ResourceLoader.Load<ParticleProcessMaterial>(
                "res://game/res/" + strandType.ToString() + ".tres"
            ),
        };

        node.AddChild(strand);
        return strand;
    }
}
