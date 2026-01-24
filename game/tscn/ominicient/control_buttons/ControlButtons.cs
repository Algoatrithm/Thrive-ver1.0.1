using System;
using Godot;

public partial class ControlButtons : HBoxContainer
{
    private Godot.Collections.Array<Button> Commands = new Godot.Collections.Array<Button>();

    public void AddCommand(String commandName, Callable func)
    {
        Button but = new Button
        {
            Text = commandName,
            CustomMinimumSize = new Vector2(100, 0),
            ButtonGroup = ResourceLoader.Load<ButtonGroup>(
                "res://game/res/command_button_group.tres"
            ),
        };
        but.AddThemeStyleboxOverride("focus", new StyleBoxEmpty());
        but.Connect(Button.SignalName.Pressed, func);
        Commands.Add(but);

        int commandsCount = Commands.Count;
        for (int i = 0; i < commandsCount; i++)
        {
            AddChild(Commands[i]);
        }
    }

    public void ClearCommands()
    {
        Commands.Clear();
        int commandsCount = GetChildCount();
        for (int i = 0; i < commandsCount; i++)
        {
            GetChild(i).QueueFree();
        }
    }
}
