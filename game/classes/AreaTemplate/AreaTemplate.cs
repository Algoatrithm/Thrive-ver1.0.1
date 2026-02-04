using System;
using Godot;

public partial class AreaTemplate : Node2D
{
    [Export]
    public Godot.Collections.Dictionary<_Variables, Variant> Param =
        new Godot.Collections.Dictionary<_Variables, Variant>
        {
            { _Variables.HasCoastLine, false },
            { _Variables.HasLake, false },
            { _Variables.HasRiver, false },
        };

    public enum _Variables
    {
        HasCoastLine,
        HasLake,
        HasRiver,
        None,
    }

    private bool HasGeneratedDomain = false;
    private AreaDomainTemplate AreaDomain;

    public void OnAreaClicked()
    {
        if (!HasGeneratedDomain)
        {
            PackedScene _PackedScene = ResourceLoader.Load<PackedScene>(
                "res://game/tscn/regions/AreaTemplate/area_domain_template.tscn"
            );
            AreaDomain = (AreaDomainTemplate)_PackedScene.Instantiate();
            AreaDomain.GenerateArea();
            HasGeneratedDomain = true;
        }
        WorldServer.Instance.SwitchToAreaDomain(AreaDomain);
    }
}
