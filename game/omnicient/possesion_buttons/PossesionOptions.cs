using System;
using Godot;

public partial class PossesionOptions : VBoxContainer
{
    public void ShowButtons()
    {
        Show();
        WorldServer.Instance.CallMethod(
            "OmnicientControl",
            "SetCommandDetails",
            "Select a type of Possesion you would like to perform."
        );
        WorldServer.Instance.CallMethod("OmnicientControl", "AddCancelButton", this);
    }

    public void HideButtons()
    {
        WorldServer.Instance.CallMethod("OmnicientControl", "SetCommandDetails", "");
        Hide();
    }
}
