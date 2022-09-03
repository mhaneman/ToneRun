using Godot;
using System;
using System.Collections.Generic; 

public class Belt : Spatial 
{
	Random rand = new Random();
	
	public PoolQueue<StaticBody, Area> platformQueue;
	public ulong distance {get; set;} = 0;
	private float speed;
	
	Area Despawn;
	Area Spawn;
	
	public void Init(float speed)
	{
		this.speed = speed;
	}
	
	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}
	
	private Singleton st;
	public override void _Ready() 
	{	
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("PlayerDied", this, "_on_Player_Died");
		
		Despawn = GetNode<Area>("Despawn");
		Spawn = GetNode<Area>("Spawn");
		
		
		Despawn.Connect("body_exited", this, "_on_body_exited");
		Despawn.Connect("area_exited", this, "_on_area_exited");
		Spawn.Connect("LowSpawn", this, "_on_Spawn_LowSpawn");
		
		platformQueue = new PoolQueue<StaticBody, Area>(this);
		
		// platforms
		platformQueue.AddCar("res://scenes/Platforms/Platform.tscn", 15);
		platformQueue.AddCar("res://scenes/Platforms/Stair.tscn", 15);
		platformQueue.AddCar("res://scenes/Platforms/Empty.tscn", 15);
		
		// fruits
		platformQueue.AddBreed("res://scenes/Items/Fruits.tscn", 200);
	}
	
	public override void _Process(float delta) 
	{
		if (distance == 0)
			platformQueue.Enqueue(0, new Vector3(3f, 1f, 5f));
		distance++;
	}
	
	/* linearly interpolate this movement for no glitching */
	/* should change to only move active objects */
	/* https://docs.godotengine.org/en/stable/tutorials/math/interpolation.html */
	public override void _PhysicsProcess(float delta)
	{
		Transform t;
		foreach(var i in platformQueue.Train)
		{
			t = i.Transform;
			t.origin.z += speed * delta;
			i.Transform = t;
		}
		
		foreach(var i in platformQueue.Riders)
		{
			t = i.Transform;
			t.origin.z += speed * delta;
			i.Transform = t;
		}
		
	}
	
	private void _on_body_exited(object body)
	{
		platformQueue.Dequeue((StaticBody) body);
	}
	
	private void _on_area_exited(object body)
	{
		platformQueue.Dequeue((Area) body);
	}
	
	private void _on_Spawn_LowSpawn()
	{
		if (distance > 0)
			RandomPlatform();
	}
	
	private void _on_Player_Died()
	{
		//distance = 0;
	}
	
	private Vector3 RandomScaleX()
	{
		float rnd = (float) rand.NextDouble();
		
		if (rnd < 0.1)
			return new Vector3(0.8f, 1f, 1f);
			
		return new Vector3(3f, 1f, 1f);
	}
	
	private Vector3 RandomHeightPlatform()
	{
		float rnd = (float) rand.NextDouble();
		if (rnd < 0.33)
			return new Vector3(1f, 0.05f, 1f);
		
		if (rnd < 0.8)
			return new Vector3(1f, 2f, 1f);
		
		return new Vector3(1f, 1f, 1f);
	}
	
	private Vector3 RandomLengthPlatform()
	{
		float rnd = (float) rand.NextDouble();
		if (rnd < 0.2)
			return new Vector3(1f, 1f, 1f);
			
		if (rnd < 0.8)
			return new Vector3(1f, 1f, 3f);
			
		return new Vector3(1f, 1f, 5f);
	}
	
	private Vector3 RandomScaleStair()
	{
		float rnd = (float) rand.NextDouble();
		if (rnd < 0.3)
			return new Vector3(1f, 0.5f, 1f);
			
		if (rnd < 0.6)
			return new Vector3(1f, 1f, 1f);
		
		return new Vector3(1f, 2f, 2f);
	}
	
	private void RandomPlatform()
	{
		float rnd = (float) rand.NextDouble();
		float rnd2 = (float) rand.NextDouble();
		
		if (rnd < 0.5)
		{
			platformQueue.Enqueue(1, RandomScaleX() * RandomScaleStair());
			platformQueue.Enqueue(0, RandomScaleX() * RandomHeightPlatform() * RandomLengthPlatform());
		} 
		else if (rnd < 0.95) 
		{
			platformQueue.Enqueue(0, RandomScaleX() * RandomHeightPlatform() * RandomLengthPlatform());
			if (rnd2 < 0.25)
				platformQueue.EnqueueRiders(0);	
		}
		else
		{
			platformQueue.Enqueue(2, RandomScaleX() * RandomHeightPlatform() * RandomLengthPlatform());	
		}
		
	}
}
