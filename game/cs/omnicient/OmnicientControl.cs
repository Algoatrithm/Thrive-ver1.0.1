using System;
using Godot;

public partial class OmnicientControl : CanvasLayer
{
	Node2D ClickedObject;
	Camera2D Camera;
	HBoxContainer ControlButtons;

	Godot.Collections.Array<Callable> Callables;

	bool LockedIn = false;
	bool AwaitingInput = false;

	public override void _Ready()
	{
		WorldServer.Instance.ObjectPassed += OnObjectPassed;
		Camera = GetNode<Camera2D>("Camera2D");
		ControlButtons = (HBoxContainer)FindChild("ControlButtons");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (LockedIn)
		{
			Camera.GlobalPosition = ClickedObject.GlobalPosition;
		}
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
}
