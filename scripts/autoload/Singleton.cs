using Godot;
using System;

public class Singleton : Node
{

	// refactor these to the world enviroment 
	[Signal]public delegate void PlayerDied();
	[Signal]public delegate void ChangeColor(int color);
	
	/* user settings */
	bool AudioEnabled = true;
	bool MusicEnabled = true;
	bool SoundFXEnabled = true;

	float AudioLevel = 1f;
	float MusicLevel = 1f;
	float SoundFXLevel = 1f;


	/* user globals */
	int Highscore = 0;
	int TotalFruits = 0;

	// refactor to the world enviroment
	/* game globals */
	public int current_color = 3;
	public int NumOfLanes = 3;
	public float PlatformSpacing = 8f;
	public float FruitSpacing = 4f;
	public float MaxPlatformWidth = 2f;
	public float InitalSpeed = 30f;
	public float MaxSpeed = 70f;
	public float SpeedInc = 0.002f;
	public int fruits = 0;
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
