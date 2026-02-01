using System;
using System.Threading.Tasks;
using Godot;

public partial class Animal : Node2D
{
    [Export]
    public float MovementSpeed { get; set; } = 20.0f;
    NavigationAgent2D _navigationAgent;
    public Godot.Collections.Array<Callable> Commands = new Godot.Collections.Array<Callable>();
    public float[] StrandParameters = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    private float MaxStrandParameter = 6.0f;
    public GpuParticles2D[] StrandReference = { null, null, null, null, null, null };
    private float StrandParametersDefault = 1.0f;

    private float _movementDelta;
    bool IsUserControlled = false;
    bool IsPossesed = false;
    bool AnimalStarted = false;
    bool IsAnimalCreatedBefore = false;

    public void StartAnimal()
    {
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _navigationAgent.VelocityComputed += OnVelocityComputed;
        ((TouchScreenButton)FindChild("TouchScreenButton")).Pressed += OnPressed;

        CreateStrand();
        UpdateStrand();

        AnimalStarted = true;
        if (!IsAnimalCreatedBefore)
            IsAnimalCreatedBefore = true;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest) { }
    }

    private void CreateStrand()
    {
        if (!IsAnimalCreatedBefore)
        {
            StrandParameters[(int)WorldServer.StrandType.vitality] = (float)
                GD.RandRange((double)StrandParametersDefault, (double)MaxStrandParameter);

            StrandParameters[(int)WorldServer.StrandType.anger] = (float)
                GD.RandRange((double)StrandParametersDefault, (double)MaxStrandParameter);

            StrandParameters[(int)WorldServer.StrandType.disgust] = (float)
                GD.RandRange((double)StrandParametersDefault, (double)MaxStrandParameter);

            StrandParameters[(int)WorldServer.StrandType.fear] = (float)
                GD.RandRange((double)StrandParametersDefault, (double)MaxStrandParameter);

            StrandParameters[(int)WorldServer.StrandType.happines] = (float)
                GD.RandRange((double)StrandParametersDefault, (double)MaxStrandParameter);

            StrandParameters[(int)WorldServer.StrandType.sadness] = (float)
                GD.RandRange((double)StrandParametersDefault, (double)MaxStrandParameter);
        }

        StrandReference[(int)WorldServer.StrandType.vitality] = WorldServer.Instance.CreateStrand(
            WorldServer.StrandType.vitality,
            this
        );
        StrandReference[(int)WorldServer.StrandType.anger] = WorldServer.Instance.CreateStrand(
            WorldServer.StrandType.anger,
            this
        );
        StrandReference[(int)WorldServer.StrandType.disgust] = WorldServer.Instance.CreateStrand(
            WorldServer.StrandType.disgust,
            this
        );
        StrandReference[(int)WorldServer.StrandType.fear] = WorldServer.Instance.CreateStrand(
            WorldServer.StrandType.fear,
            this
        );
        StrandReference[(int)WorldServer.StrandType.happines] = WorldServer.Instance.CreateStrand(
            WorldServer.StrandType.happines,
            this
        );
        StrandReference[(int)WorldServer.StrandType.sadness] = WorldServer.Instance.CreateStrand(
            WorldServer.StrandType.sadness,
            this
        );
    }

    public void IncrementStrand(WorldServer.StrandType strandType, float amount = 0.1f)
    {
        if (StrandParameters[(int)strandType] > MaxStrandParameter)
        {
            WorldServer.Instance.CallMethod(
                "OmnicientControl",
                "SetWarning",
                "Maximum Length Maxed: " + MaxStrandParameter
            );
            return;
        }
        StrandParameters[(int)strandType] += amount;
        WorldServer.Instance.CallMethod(
            "OmnicientControl",
            "SetWarning",
            StrandParameters[(int)strandType]
        );
        UpdateStrand();
    }

    public void DecrementStrand(WorldServer.StrandType strandType, float amount = 0.1f)
    {
        if (StrandParameters[(int)strandType] < StrandParametersDefault)
            return;

        StrandParameters[(int)strandType] -= amount;
        UpdateStrand();
    }

    public void UpdateStrand()
    {
        StrandReference[(int)WorldServer.StrandType.vitality].Lifetime =
            StrandParameters[(int)WorldServer.StrandType.vitality] + StrandParametersDefault;
        StrandReference[(int)WorldServer.StrandType.anger].Lifetime =
            StrandParameters[(int)WorldServer.StrandType.anger] + StrandParametersDefault;
        StrandReference[(int)WorldServer.StrandType.disgust].Lifetime =
            StrandParameters[(int)WorldServer.StrandType.disgust] + StrandParametersDefault;
        StrandReference[(int)WorldServer.StrandType.fear].Lifetime =
            StrandParameters[(int)WorldServer.StrandType.fear] + StrandParametersDefault;
        StrandReference[(int)WorldServer.StrandType.happines].Lifetime =
            StrandParameters[(int)WorldServer.StrandType.happines] + StrandParametersDefault;
        StrandReference[(int)WorldServer.StrandType.sadness].Lifetime =
            StrandParameters[(int)WorldServer.StrandType.sadness] + StrandParametersDefault;

        for (int i = 0; i < (int)WorldServer.StrandType.end; i++)
        {
            double n = StrandReference[i].Lifetime;
            if (n >= 1 && n < 2)
            {
                ((ParticleProcessMaterial)StrandReference[i].ProcessMaterial).EmissionShapeOffset =
                    new Vector3(-12.0f, 0.0f, 0.0f);
            }
            else if (n >= 2 && n < 3)
            {
                ((ParticleProcessMaterial)StrandReference[i].ProcessMaterial).EmissionShapeOffset =
                    new Vector3(-25.0f, 0.0f, 0.0f);
            }
            else if (n >= 3 && n < 4)
            {
                ((ParticleProcessMaterial)StrandReference[i].ProcessMaterial).EmissionShapeOffset =
                    new Vector3(-33.0f, 0.0f, 0.0f);
            }
            else if (n >= 4 && n < 5)
            {
                ((ParticleProcessMaterial)StrandReference[i].ProcessMaterial).EmissionShapeOffset =
                    new Vector3(-41.0f, 0.0f, 0.0f);
            }
            else if (n >= 5 && n < 6)
            {
                ((ParticleProcessMaterial)StrandReference[i].ProcessMaterial).EmissionShapeOffset =
                    new Vector3(-70.0f, 0.0f, 0.0f);
            }
            else if (n >= 6 && n < 7)
            {
                ((ParticleProcessMaterial)StrandReference[i].ProcessMaterial).EmissionShapeOffset =
                    new Vector3(-100.0f, 0.0f, 0.0f);
            }
        }
    }

    public void RegisterCommand(StringName methodName)
    {
        Commands.Add(new Callable(this, methodName));
    }

    private void OnPressed()
    {
        if (IsPossesed)
        {
            IsPossesed = false;
            WorldServer.Instance.ClearObject();
            WorldServer.Instance.CallMethod("OmnicientControl", "SetCommandDetails", "");
            return;
        }

        WorldServer.Instance.CallMethod("PossesionOptions", "ShowButtons");
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
