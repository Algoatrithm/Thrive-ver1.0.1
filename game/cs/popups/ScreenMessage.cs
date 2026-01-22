using System;
using Godot;

public partial class ScreenMessage : CanvasLayer
{
	public void SetText(string text)
    {
        ((RichTextLabel)FindChild("RichTextLabel")).Text = text;
    }
    public void Destroy()
    {
        QueueFree();
    }

    public override void _Ready() { }
}
