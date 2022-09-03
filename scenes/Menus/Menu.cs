using Godot;
using System;

public class Menu : Control
{
	AudioStreamPlayer AudioStartup;
	public override void _Ready()
	{
		AudioStartup = GetNode<AudioStreamPlayer>("GumGumStartup");	
	}

	private void _on_StartButton_pressed()
	{
		AudioStartup.Play();
		GetTree().ChangeScene("res://scenes/World.tscn");
	}
	
	private void _on_GumGumStartup_finished()
	{
	}
	
	private void _on_SettingsButton_pressed()
	{
		// Replace with function body.
	}

}
