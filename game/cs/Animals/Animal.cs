using System;
using System.Threading.Tasks;
using Godot;

public partial class Animal : Node2D
{
    [Export]
    public float MovementSpeed { get; set; } = 20.0f;
    NavigationAgent2D _navigationAgent;
    public Godot.Collections.Array<Callable> Commands = new Godot.Collections.Array<Callable>();

    private float _movementDelta;
    bool IsUserControlled = false;
    bool IsPossesed = false;
    bool AnimalStarted = false;

    public override void _Ready() { }

    public override void _Input(InputEvent @event) { }

    public void StartAnimal()
    {
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _navigationAgent.VelocityComputed += OnVelocityComputed;
        ((TouchScreenButton)FindChild("TouchScreenButton")).Pressed += OnPressed;

        AnimalStarted = true;
    }

    public void RegisterCommand(StringName methodName)
    {
        Commands.Add(new Callable(this, methodName));
    }

    private void OnPressed()
    {
        if (IsPossesed)
            return;
        IsPossesed = true;
        WorldServer.Instance.SetObject(this, Commands);
    }

    public virtual void AnimalMovement() { }

    public void SetMovementTarget(Vector2 movementTarget)
    {
        _navigationAgent.TargetPosition = movementTarget;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!AnimalStarted)
        {
            GD.PushError(
                "Animal \""
                    + Name
                    + "\" has not called \"StartAnimal()\" method on the _Ready() function. The Animal will not behave as intended."
            );
            return;
        }
        // Do not query when the map has never synchronized and is empty.
        if (NavigationServer2D.MapGetIterationId(_navigationAgent.GetNavigationMap()) == 0)
        {
            return;
        }

        if (_navigationAgent.IsNavigationFinished())
        {
            AnimalMovement();
            return;
        }

        _movementDelta = MovementSpeed * (float)delta;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();
        Vector2 newVelocity = GlobalPosition.DirectionTo(nextPathPosition) * _movementDelta;
        if (_navigationAgent.AvoidanceEnabled)
        {
            _navigationAgent.Velocity = newVelocity;
        }
        else
        {
            OnVelocityComputed(newVelocity);
        }
    }

    private void OnVelocityComputed(Vector2 safeVelocity)
    {
        GlobalPosition = GlobalPosition.MoveToward(GlobalPosition + safeVelocity, _movementDelta);
    }
}
