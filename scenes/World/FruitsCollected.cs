using Godot;
using System;

public class FruitsCollected : Label
{
	private Singleton st;
	public override void _Ready()
	{
		st = GetNode<Singleton>("/root/Singleton");
	}
	
	public override void _Process(float delta)
	{
		this.Text = "Fruits " + st.fruits.ToString();
	}
}
