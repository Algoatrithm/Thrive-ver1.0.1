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
    }

    public void HideButtons()
    {
        Hide();
    }
}
