using Godot;
using System;

public class FPS : Label
{
	public override void _Ready() 
	{
		this.Text = "setup...";
	}
	
	public override void _Process(float delta) 
	{
		this.Text = "FPS: " + Convert.ToString(Performance.GetMonitor(0));
	}
}
