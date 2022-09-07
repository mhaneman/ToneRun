using Godot;
using System;

public class PauseMenu : CanvasLayer
{
	Button Quit;
	Button Resume;
	public override void _Ready()
	{
		Quit = GetNode<Button>("Quit");
		Resume = GetNode<Button>("Resume");
		
		Quit.Visible = false;
		Resume.Visible = false;
	}
	
	private void _OnPausePressed()
	{
		GetTree().Paused = true;
		Quit.Visible = true;
		Resume.Visible = true;
	}
	
	private void _OnResumePressed()
	{
		Quit.Visible = false;
		Resume.Visible = false;
		GetTree().Paused = false;
	}
	
	private void _OnQuitPressed()
	{
		GetTree().ChangeScene("res://scenes/World/Intro.tscn");
	}
	
	
}
