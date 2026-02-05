using System;
using Godot;

public partial class Entity : Node2D
{
    /*
        Basics of an Entity

        WanderAround() - Randomly move the entity
        Procure() - Procures an object
        Scout() - Reports information
            
    
    */
    private float MovementSpeed = 1000;
    private STATE_MACHINE CurrentState = STATE_MACHINE.IDLE;
    private NavigationAgent2D Movement;
    private AnimatedSprite2D Appearance;
    private Label Indicator;
    private float _movementDelta;
    private UtilityServer.OBJECT LookingFor;

    private enum STATE_MACHINE
    {
        IDLE,
        WALK,
        RUN,
    }

    public void WanderAround(float radius = 1000)
    {
        Vector2 rand_pos = new Vector2(
            (float)GD.RandRange(radius * -1.0, radius),
            (float)GD.RandRange(radius * -1.0, radius)
        );
        SetState(STATE_MACHINE.WALK);
        Movement.TargetPosition = GlobalPosition + rand_pos;
    }

    public void Goto(Vector2 where)
    {
        Movement.TargetPosition = where;
    }

    public void Procure(
        bool foundAlready,
        UtilityServer.OBJECT what,
        UtilityServer.STORAGE putWhere
    )
    {
        // What object
        SetLookingFor(what);
        // Is it found already or needs to find it
        WanderAround();
        // Where to put
    }

    public void Scout()
    {
        // wander around and report whatever's found
    }

    private float GetSpeed()
    {
        return MovementSpeed;
    }

    private void SetSpeed(float new_speed)
    {
        MovementSpeed = new_speed;
    }

    private void SetLookingFor(UtilityServer.OBJECT what)
    {
        LookingFor = what;
    }

    private void SetState(STATE_MACHINE new_state)
    {
        CurrentState = new_state;
        Indicator.Text = CurrentState.ToString();
        if (new_state == STATE_MACHINE.IDLE)
            MovementSpeed = MovementSpeed * 2;
        if (new_state == STATE_MACHINE.WALK)
            MovementSpeed = MovementSpeed / 2;
    }

    public override void _Ready()
    {
        Movement = GetChild<NavigationAgent2D>(0);
        if (Movement == null)
            GD.PrintErr("NavigationAgent2D should be the 0 index node of the entity scene");
        Appearance = GetChild<AnimatedSprite2D>(1);
        if (Appearance == null)
            GD.PrintErr("AnimatedSprite2D should be the 1 index node of the entity scene");
        Indicator = GetChild<Label>(2);
        if (Indicator == null)
            GD.PrintErr("Label should be the 2 index node of the entity scene");
        Movement.VelocityComputed += OnVelocityComputed;

        //WanderAround();
    }

    public override void _Process(double delta) { }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        // Movement

        if (NavigationServer2D.MapGetIterationId(Movement.GetNavigationMap()) == 0)
            return;

        if (Movement.IsNavigationFinished())
        {
            SetState(STATE_MACHINE.IDLE);
            WanderAround();
            return;
        }

        CallDeferred("StateMachine", delta);
    }

    private void StateMachine(double _delta)
    {
        switch (CurrentState)
        {
            case STATE_MACHINE.IDLE:
                break;
            case STATE_MACHINE.WALK:
                _movementDelta = MovementSpeed * (float)_delta;
                Vector2 nextPathPosition = Movement.GetNextPathPosition();
                Vector2 newVelocity = GlobalPosition.DirectionTo(nextPathPosition) * _movementDelta;
                Movement.Velocity = newVelocity;
                if (Movement.AvoidanceEnabled)
                {
                    Movement.Velocity = newVelocity;
                }
                else
                {
                    OnVelocityComputed(newVelocity);
                }
                break;
            case STATE_MACHINE.RUN:
                break;
        }
    }

    private void OnVelocityComputed(Vector2 safeVelocity)
    {
        GlobalPosition = GlobalPosition.MoveToward(GlobalPosition + safeVelocity, _movementDelta);
    }
}
