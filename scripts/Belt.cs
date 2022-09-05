using Godot;
using System;

public class Belt : Spatial 
{
	Random rand = new Random();
	
	public PoolQueue<StaticBody> platformQueue;
	public ulong distance {get; set;} = 0;
	private float Speed;
	private float MaxSpeed;
	private float SpeedInc;
	private int NumOfLanes;
	private float PlatformSpacing;
	private float MaxPlatformWidth;
	
	Area Despawn;
	Area Spawn;
	
	private Singleton st;
	public override void _Ready() 
	{	
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("PlayerDied", this, "on_PlayerDied");
		this.NumOfLanes = st.NumOfLanes;
		this.PlatformSpacing = st.PlatformSpacing;
		this.MaxPlatformWidth = st.MaxPlatformWidth;
		this.Speed = st.InitalSpeed;
		this.MaxSpeed = st.MaxSpeed;
		this.SpeedInc = st.SpeedInc;
		
		Despawn = GetNode<Area>("Despawn");
		Despawn.Connect("body_exited", this, "on_DespawnPlatformArea");
		
		Spawn = GetNode<Area>("Spawn");
		Spawn.Connect("LowSpawn", this, "on_SpawnNewPlatformsArea");
		
		platformQueue = new PoolQueue<StaticBody>(this, NumOfLanes);
		
		// platforms
		platformQueue.AddObjType("stair", "res://scenes/Platforms/Stair.tscn", 15);
		platformQueue.AddObjType("down", "res://scenes/Platforms/Down.tscn", 15);
		platformQueue.AddObjType("flat", "res://scenes/Platforms/Flat.tscn", 15);
		platformQueue.AddObjType("up", "res://scenes/Platforms/Up.tscn", 15);
		platformQueue.AddObjType("gap", "res://scenes/Platforms/Gap.tscn", 15);
	}
	
	public override void _PhysicsProcess(float delta) 
	{
		if (distance == 0)
			InitalizeLanes();

		platformQueue.MoveObjects(Speed, delta);
		distance++;
		if (Speed < MaxSpeed)
			Speed += SpeedInc;
	}

	private void InitalizeLanes()
	{
		Vector3 scale = new Vector3(MaxPlatformWidth, 1f, 10f);
		(string, Vector3, float)[] InitalPlatforms = new (string, Vector3, float)[NumOfLanes];
		for (int n=0; n<NumOfLanes; n++)
		{
			int placement = n - (NumOfLanes / 2);
			InitalPlatforms[n] = ("flat", scale, placement * PlatformSpacing);
		}
		platformQueue.InitalizePlatforms(InitalPlatforms);
	}
	
	private void SummonPlatformRow()
	{
		(string, Vector3)[] Platforms = new (string, Vector3)[NumOfLanes];
		float[] Heights = new float[NumOfLanes];
		for(int n=0; n<NumOfLanes; n++)
			Heights[n] = platformQueue.GetHeight(n);
		
	
		for (int n=0; n<NumOfLanes; n++)
		{
			string type = RandomPlatformType(Heights[n], n);
			Platforms[n] = (type, new Vector3(MaxPlatformWidth, 1f, 2f) * RandomWidth());
		}
		
		platformQueue.Enqueue(Platforms);
	}

	private string RandomPlatformType(float Height, int n)
	{
		//stairs can only connect to flats
		string prev_type = platformQueue.Head[n].Filename;
		if (Height <= 0f)
			return "stair";
		else if (Height >= 25f)
			return "down";
		else if (prev_type == "res://scenes/Platforms/Stair.tscn" || prev_type == "res://scenes/Platforms/Down.tscn")
			return "flat";
		else if (prev_type == "res://scenes/Platforms/Flat.tscn" && rand.Next(2) != 0)
			return "flat";
		else if (prev_type == "res://scenes/Platforms/Gap.tscn" && rand.Next(3) != 0)
			return "gap";
		else if (rand.Next(2) != 0)
			return "stair";
		else if (rand.Next(4) != 0)
			return "down";
		else if (rand.Next(4) != 0)
			return "up";
		else if (rand.Next(2) != 0)
			return "gap";
		
		return "flat";
	}

	private Vector3 RandomWidth()
	{
		float w1 = (float) rand.NextDouble();
		if (w1 < 0.1)
			return new Vector3(0.25f, 1f, 1f);
		if (w1 < 0.2)
			return new Vector3(0.5f, 1f, 1f);
		return Vector3.One;
	}
	
	/* ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ */
	/* Signals */
	
	private void on_DespawnPlatformArea(object body)
	{
		platformQueue.Dequeue((StaticBody) body);
	}
	
	private void on_SpawnNewPlatformsArea()
	{
		if (distance > 0)
			SummonPlatformRow();
	}
	
	private void on_PlayerDied()
	{
		//distance = 0;
	}
}
