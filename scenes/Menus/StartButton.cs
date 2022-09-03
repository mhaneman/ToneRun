using Godot;
using System;

public class StartButton : Button
{
	private void _on_StartButton_pressed()
	{
		GetTree().ChangeScene("res://scenes/World/World.tscn");
	}
}
