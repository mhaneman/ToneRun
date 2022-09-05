using Godot;
using System;

public class Belt : Spatial 
{
	Random rand = new Random();
	
	public PoolQueue<StaticBody> platformQueue;
	public ulong distance {get; set;} = 0;
	private float Speed = 40f;
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
		
		Despawn = GetNode<Area>("Despawn");
		Despawn.Connect("body_exited", this, "on_DespawnPlatformArea");
		
		Spawn = GetNode<Area>("Spawn");
		Spawn.Connect("LowSpawn", this, "on_SpawnNewPlatformsArea");
		
		platformQueue = new PoolQueue<StaticBody>(this, NumOfLanes);
		
		// platforms
		platformQueue.AddObjType("res://scenes/Platforms/Stair.tscn", 15);
		platformQueue.AddObjType("res://scenes/Platforms/Down.tscn", 15);
		platformQueue.AddObjType("res://scenes/Platforms/Flat.tscn", 15);
		platformQueue.AddObjType("res://scenes/Platforms/Up.tscn", 15);
		platformQueue.AddObjType("res://scenes/Platforms/Gap.tscn", 15);
	}
	
	public override void _PhysicsProcess(float delta) 
	{
		if (distance == 0)
			InitalizeLanes();

		platformQueue.MoveObjects(Speed, delta);
		distance++;
	}

	private void InitalizeLanes()
	{
		int type = 2;
		Vector3 scale = new Vector3(MaxPlatformWidth, 1f, 10f);
		(int, Vector3, float)[] InitalPlatforms = new (int, Vector3, float)[NumOfLanes];
		for (int n=0; n<NumOfLanes; n++)
		{
			int placement = n - (NumOfLanes / 2);
			InitalPlatforms[n] = (type, scale, placement * PlatformSpacing);
		}
		platformQueue.InitalizePlatforms(InitalPlatforms);
	}
	
	private void SummonPlatformRow()
	{
		(int, Vector3)[] Platforms = new (int, Vector3)[NumOfLanes];
		float[] Heights = new float[NumOfLanes];
		for(int n=0; n<NumOfLanes; n++)
			Heights[n] = platformQueue.GetHeight(n);
		
	
		for (int n=0; n<NumOfLanes; n++)
		{
			int type = RandomPlatformType(Heights[n], n);
			Platforms[n] = (type, new Vector3(MaxPlatformWidth, 1f, 2f) * RandomWidth());
		}
		
		platformQueue.Enqueue(Platforms);
	}

	private int RandomPlatformType(float Height, int n)
	{
		//stairs can only connect to flats
		int type;
		if (Height <= 0f)
			type = 0;
		else if (Height >= 15f)
			type = 1;
		else if (platformQueue.Head[n].Filename == "res://scenes/Platforms/Stair.tscn" || 
			platformQueue.Head[n].Filename == "res://scenes/Platforms/Down.tscn" ||
			platformQueue.Head[n].Filename == "res://scenes/Platforms/Gap.tscn")
		{
			type = 2;
		}
		else
		{
			float w1 = (float) rand.NextDouble();
			if (w1 < 0.1)
				type = rand.Next(2) == 0 ? 0 : 3;
			else if (w1 < 0.2)
				type = 1;
			else
				type = rand.Next(2) == 0 ? 2 : 4;
		}

		return type;
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
