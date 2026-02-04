using System;
using System.Threading.Tasks;
using Godot;

public partial class Eagle : Animal
{
    /*
            To Do:
            - Flight Time
            - Land
            - Travel
    
    */
    float a = 0;
    float radius = 11.0f;
    float DescendAndAscendTime = 5f;
    bool IsScouting = false;
    bool IsOnAir = false;
    bool IsTakingOff = false;
    bool IsLanding = false;
    bool HasLanded = true;
    bool IsTraveling = false;
    bool IsUserControlled = false;
    bool IsPossesed = false;

    public override void _Ready()
    {
        StartAnimal();
        RegisterCommand(MethodName.Travel);
        TakeOff();
        Scout();
    }

    public override void _OnPressedPossesed()
    {
        WorldServer.Instance.SetSetting(WorldServer.SettingKeys.LockZoom, true);
        WorldServer.Instance.CallMethod(
            "OmnicientsView",
            "SetFixedCameraZoom",
            new Vector2(17, 17)
        );
    }

    public override void _Input(InputEvent @event)
    {
        if (IsUserControlled && @event.IsActionPressed("Click"))
            CallDeferred("SetTravel", GetGlobalMousePosition());
    }

    public async void TakeOff()
    {
        if (IsOnAir)
            return;
        IsTakingOff = true;
        Tween tween = GetTree().CreateTween();
        MovementSpeed = MovementSpeed / 3;
        tween
            .TweenProperty(
                this,
                "scale",
                new Vector2(0.15f, 0.15f),
                GD.RandRange(2, DescendAndAscendTime)
            )
            .SetTrans(Tween.TransitionType.Sine);
        await ToSignal(tween, Tween.SignalName.Finished);
        IsScouting = false;
        IsOnAir = true;
        IsTakingOff = false;
        IsLanding = false;
        HasLanded = false;
        IsTraveling = false;
        MovementSpeed = MovementSpeed * 3;
        Scout();
    }

    public void Scout()
    {
        if (HasLanded || IsLanding)
            return;
        GD.Print("Scouting");
        SetMovementTarget(GlobalPosition + new Vector2(0f, 0f));
        IsScouting = true;
        IsOnAir = true;
        IsTakingOff = false;
        IsLanding = false;
        HasLanded = false;
        IsTraveling = false;
        IsUserControlled = false;
    }

    public async void Land()
    {
        if (HasLanded)
            return;
        SetMovementTarget(GlobalPosition + new Vector2(0f, 0f));
        IsOnAir = true;
        IsTakingOff = false;
        IsLanding = true;
        GD.Print("Landing");
        Tween tween = GetTree().CreateTween();
        MovementSpeed = MovementSpeed / 2;
        tween
            .TweenProperty(
                this,
                "scale",
                new Vector2(0.1f, 0.1f),
                GD.RandRange(2, DescendAndAscendTime)
            )
            .SetTrans(Tween.TransitionType.Sine);
        await ToSignal(tween, Tween.SignalName.Finished);
        HasLanded = true;
        IsLanding = false;
        IsOnAir = false;
        IsScouting = false;
        IsTraveling = false;
        MovementSpeed = MovementSpeed * 2;
    }

    // Causes bug, may have to disable this or maybe for future use
    public void Travel()
    {
        if (HasLanded)
            return;
        //await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        WorldServer.Instance.CallMethod(
            "OmnicientControl",
            "SetCommandDetails",
            "Click on any area on the screen to move the creature.\nThe creature will move to the newest assigned position and goes back\nto its default behaviour once it reaches the newest assigned position."
        );

        IsUserControlled = true;
    }

    public void SetTravel(Vector2 pos)
    {
        SetMovementTarget(pos);
        IsTraveling = true;
        IsScouting = false;
        IsOnAir = true;
        IsTakingOff = false;
        IsLanding = false;
        HasLanded = false;
    }

    public override void AnimalMovement()
    {
        if (IsScouting && IsOnAir)
        {
            SetMovementTarget(
                GlobalPosition + new Vector2((float)Mathf.Sin(a), (float)Mathf.Cos(a)) * radius
            );
            if (a >= 6.28)
                a = 0;
            a += 0.1f;
        }
        else if (IsTraveling)
        {
            Scout();
            WorldServer.Instance.CallMethod(
                "OmnicientControl",
                "SetCommandDetails",
                "Select any available command to control or interact with the creature."
            );
        }
        return;
    }
}
