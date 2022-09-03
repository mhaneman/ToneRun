using Godot;
using System;

public class Belt : Spatial 
{
	Random rand = new Random();
	
	public PoolQueue<StaticBody> platformQueue;
	public ulong distance {get; set;} = 0;
	private float Speed = 50f;
	
	Area Despawn;
	Area Spawn;
	
	private Singleton st;
	public override void _Ready() 
	{	
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("PlayerDied", this, "on_PlayerDied");
		
		Despawn = GetNode<Area>("Despawn");
		Despawn.Connect("body_exited", this, "on_DespawnPlatformArea");
		
		Spawn = GetNode<Area>("Spawn");
		Spawn.Connect("LowSpawn", this, "on_SpawnNewPlatformsArea");
		
		platformQueue = new PoolQueue<StaticBody>(this);
		
		// platforms
		platformQueue.AddObjType("res://scenes/Platforms/Platform.tscn", 15);
		platformQueue.AddObjType("res://scenes/Platforms/Stair.tscn", 15);
		//platformQueue.AddObjType("res://scenes/Platforms/Empty.tscn", 15);
		platformQueue.AddObjType("res://scenes/Platforms/Down.tscn", 15);
	}
	
	public override void _Process(float delta) 
	{
		if (distance == 0)
		{
			(int, Vector3, float) InitalPlatformLeft = (0, new Vector3(3f, 1f, 5f), -6f);
			(int, Vector3, float) InitalPlatformMiddle = (0, new Vector3(3f, 1f, 5f), 0f);
			(int, Vector3, float) InitalPlatformRight = (0, new Vector3(3f, 1f, 5f), 6f);

			(int, Vector3, float)[] InitalPlatforms = {InitalPlatformLeft, InitalPlatformMiddle, InitalPlatformRight};
			platformQueue.InitalizePlatforms(InitalPlatforms);
		}

		platformQueue.MoveObjects(Speed, delta);
		distance++;
	}
	
	private void on_DespawnPlatformArea(object body)
	{
		platformQueue.Dequeue((StaticBody) body);
	}
	
	private void on_SpawnNewPlatformsArea()
	{
		if (distance > 0)
		{
			int type = rand.Next(3);
			(int, Vector3) Platform = (type, new Vector3(3f, 1f, 3f));
			(int, Vector3)[] Platforms = {Platform, Platform, Platform};
			platformQueue.Enqueue(Platforms);
		}
	}
	
	private void on_PlayerDied()
	{
		//distance = 0;
	}
}
