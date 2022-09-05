using Godot;
using System;

public class Singleton : Node
{
	[Signal]public delegate void PlayerDied();
	[Signal]public delegate void ChangeColor(int color);
	
	Random rand = new Random();
	
	public int current_color;
	public int NumOfLanes = 3;
	public float PlatformSpacing = 4f;
	public float MaxPlatformWidth = 1f;
	public float speed;
	public int fruits = 0;
	
	public override void _Ready()
	{
		current_color = rand.Next(0, 6);
		speed = 50f;
	}
}



public enum Colors 
{
	Red,
	Orange,
	Yellow,
	Green,
	Blue,
	Purple,
	Black
}
