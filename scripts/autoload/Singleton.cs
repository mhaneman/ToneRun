using Godot;
using System;

public class Singleton : Node
{
	[Signal]public delegate void PlayerDied();
	[Signal]public delegate void ChangeColor(int color);
	
	Random rand = new Random();
	
	public int current_color;
	public int NumOfLanes = 3;
	public float PlatformSpacing = 8f;
	public float FruitSpacing = 4f;
	public float MaxPlatformWidth = 2f;
	public float InitalSpeed = 30f;
	public float MaxSpeed = 70f;
	public float SpeedInc = 0.002f;
	public int fruits = 0;
	
	public override void _Ready()
	{
		current_color = rand.Next(0, 6);
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
