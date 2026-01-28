using System;
using Godot;

public partial class OmnicientControl : CanvasLayer
{
    Node2D ClickedObject;
    Camera2D Camera;
    HBoxContainer ControlButtons;

    Godot.Collections.Array<Callable> Callables = new Godot.Collections.Array<Callable>();

    [Export]
    public Vector2 Boundary = new Vector2(1200, 950);
    bool LockedIn = false;
    bool AwaitingInput = false;

    public override void _Ready()
    {
        WorldServer.Instance.ObjectPassed += OnObjectPassed;
        WorldServer.Instance.ClearObjectPassed += OnClearObjectPassed;
        Camera = GetNode<Camera2D>("OmnicientsView");
        ControlButtons = (HBoxContainer)FindChild("ControlButtons");

        Camera.LimitLeft = 0;
        Camera.LimitTop = 0;
        Camera.LimitBottom = (int)Boundary.Y;
        Camera.LimitRight = (int)Boundary.X;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (LockedIn)
        {
            Camera.GlobalPosition = ClickedObject.GlobalPosition;
        }
    }

    /* Use WorldServer.Instance.CallMethod(
              "OmnicientControl",
              "SetCommandDetails",
              "Text here"
    ); For easier access */
    public void SetCommandDetails(String text)
    {
        FindChild("CommandDetails").Set("text", text);
    }

    /* Use WorldServer.Instance.CallMethod(
              "OmnicientControl",
              "SetWarning",
              "Text here"
    ); For easier access */
    public async void SetWarning(String text)
    {
        Node node = FindChild("Warning");
        node.Set("text", text);
        await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
        node.Set("text", "");
    }

    public void OnObjectPassed(Node2D node, Godot.Collections.Array<Callable> callables)
    {
        ClickedObject = node;
        LockedIn = true;
        Callables = callables;

        for (int i = 0; i < callables.Count; i++)
        {
            ControlButtons.Call("AddCommand", Callables[i].Method, Callables[i]);
        }
    }

    public void OnClearObjectPassed()
    {
        ClickedObject = null;
        LockedIn = false;
        Callables = null;

        ControlButtons.Call("ClearCommands");
    }
}
