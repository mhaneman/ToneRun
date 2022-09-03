using Godot;
using System;

public class Score : Label
{
	private int score = 0;
	
	public override void _Ready() 
	{
		this.Text = "Score: " + Convert.ToString(score);
	}
}
