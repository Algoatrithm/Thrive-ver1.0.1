using System;
using Godot;

public partial class InputServer : Node
{
    public static InputServer Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public Node NodeToAcceptInput = null;
    public bool SpecificInput = false;

    public bool VerifyInput(Node _this)
    {
        if (SpecificInput && NodeToAcceptInput != _this)
            return false;
        return true;
    }

    public void RegisterInputReciever(Node _this)
    {
        InputServer.Instance.NodeToAcceptInput = _this;
        InputServer.Instance.SpecificInput = true;
    }

    public void ResetInputReciever()
    {
        NodeToAcceptInput = null;
        SpecificInput = false;
    }
}
