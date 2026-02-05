using System;
using Godot;

public partial class PossesionOptions : VBoxContainer
{
    public void ShowButtons()
    {
        Show();
        UtilityServer.Instance.CallMethod(
            "OmnicientControl",
            "SetCommandDetails",
            "Select a type of Possesion you would like to perform."
        );
        UtilityServer.Instance.CallMethod("OmnicientControl", "AddCancelButton", this);
    }

    public void HideButtons()
    {
        UtilityServer.Instance.CallMethod("OmnicientControl", "SetCommandDetails", "");
        Hide();
    }
}
