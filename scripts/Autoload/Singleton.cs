using Godot;
using System;

public class Singleton : Node
{

	// refactor these to the world enviroment 
	[Signal]public delegate void PlayerDied();
	[Signal]public delegate void ChangeColor(int color);

	// refactor to the world enviroment
	/* game globals */
	public int CurrentColor;
	public int NumOfLanes = 3;
	public float PlatformSpacing = 8f;
	public float FruitSpacing = 4f;
	public float MaxPlatformWidth = 2f;
	public float InitalSpeed = 30f;
	public float MaxSpeed = 70f;
	public float SpeedInc = 0.002f;
	public int fruits = 0;


	public ResourceManager resourceManager;

	Random rand = new Random();
	public override void _Ready()
	{
		this.CurrentColor = rand.Next(8);
		resourceManager = new ResourceManager(CurrentColor);


		this.Connect("ChangeColor", this, "on_ChangeColor");
	}

	

	private void on_ChangeColor()
	{
		this.CurrentColor = rand.Next(8);
		resourceManager.ChangeTones(CurrentColor);
	}
}
