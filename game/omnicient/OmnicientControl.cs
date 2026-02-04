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
    Node CancelButton;

    private enum PossesionType
    {
        Full,
        Partial,
        Light,
    }

    public override void _Ready()
    {
        WorldServer.Instance.ObjectPassed += OnObjectPassed;
        WorldServer.Instance.ClearObjectPassed += OnClearObjectPassed;
        Camera = GetNode<Camera2D>("OmnicientsView");
        ControlButtons = (HBoxContainer)FindChild("ControlButtons");

        WorldServer.Instance.CallMethod("PossesionOptions", "HideButtons");

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

    public void AddCancelButton(Node targetToNegate)
    {
        if (FindChild("ControlButtons").GetChildCount() > 0)
            return;
        Node cancelButton = ResourceLoader
            .Load<PackedScene>("res://game/popups/cancel.tscn")
            .Instantiate();
        FindChild("ControlButtons").AddChild(cancelButton);
        ((Button)FindChild("ControlButtons").GetChild(0)).Pressed += () =>
            RemoveCancelButton(cancelButton, targetToNegate);
        CancelButton = cancelButton;
    }

    public void RemoveCancelButton(Node target, Node TargetToOperate)
    {
        if (FindChild("ControlButtons").GetChildCount() < 1)
            return;
        // Hide the target
        TargetToOperate.Call("HideButtons");
        // Removes the Cancel button
        target.QueueFree();
    }

    public void RemoveCancelButton()
    {
        // Removes the Cancel button
        CancelButton.QueueFree();
    }

    public void OnFullPossesion()
    {
        SetPossesionType(PossesionType.Full);
    }

    public void OnPartialPossesion()
    {
        SetPossesionType(PossesionType.Partial);
    }

    public void OnLightPossesion()
    {
        SetPossesionType(PossesionType.Light);
    }

    private void SetPossesionType(PossesionType pos)
    {
        LockedIn = true;
        WorldServer.Instance.CallMethod(
            "OmnicientControl",
            "SetCommandDetails",
            "Select any available command to control or interact with the creature."
        );
        WorldServer.Instance.CallMethod("PossesionOptions", "HideButtons");
        for (int i = 0; i < Callables.Count; i++)
        {
            ControlButtons.Call("AddCommand", Callables[i].Method, Callables[i]);
        }

        RemoveCancelButton();
        switch (pos)
        {
            case PossesionType.Full:
                break;
            case PossesionType.Partial:
                break;
            case PossesionType.Light:
                break;
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
        Callables = callables;
        ClickedObject = node;
    }

    public void OnClearObjectPassed()
    {
        ClickedObject = null;
        LockedIn = false;
        Callables = null;

        ControlButtons.Call("ClearCommands");
    }
}
