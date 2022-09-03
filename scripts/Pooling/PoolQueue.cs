using Godot;
using System;

using System.Collections.Generic;

public class PoolQueue<T, U> : Node where T : Spatial where U : Spatial
{
	/* seperate the object pool out... eventually */
	public LinkedList<T> Train = new LinkedList<T>();
	private List<PooledObject<T>> cars = new List<PooledObject<T>>();
	
	public LinkedList<U> Riders = new LinkedList<U>();
	private List<PooledObject<U>> breeds = new List<PooledObject<U>>();
	
	private Spatial Other;
	
	public PoolQueue(){}
	
	public PoolQueue(Spatial Other)
	{
		this.Other = Other;
	}
	
	public void AddCar(string ScenePath, int InitCount=1)
	{
		cars.Add(new PooledObject<T>(Other, ScenePath, InitCount));
	}
	
	public void AddBreed(string ScenePath, int InitCount=1)
	{
		breeds.Add(new PooledObject<U>(Other, ScenePath, InitCount));
	}
	
	public void EnqueueRiders(int obj_num)
	{
		if (Train == null || Train.Count == 0)
			GD.Print("Uninitalized child");
		else
		{
			Vector3 front = Train.Last.Value.GetNode<Spatial>("FruitRegion/front").GlobalTransform.origin;
			Vector3 back = Train.Last.Value.GetNode<Spatial>("FruitRegion/back").GlobalTransform.origin;
			front.y += 1;
			while(front.z >= back.z)
			{
				Transform spawnLoc = new Transform(Vector3.Left, Vector3.Up, Vector3.Forward, front);
				Riders.AddLast(breeds[obj_num].Summon(spawnLoc));
				front.z -= 6;
			}
		}
	}
	
	public void Enqueue(int obj_num, Vector3 scale)
	{
		Transform spawnLoc;
		spawnLoc = Other.GlobalTransform;
		if (Train != null && Train.Count > 0)
			spawnLoc.origin.z = 
				Train.Last.Value.GetNode<Spatial>("end").GlobalTransform.origin.z;	
		Train.AddLast(cars[obj_num].Summon(spawnLoc, scale));
	}
	
	public void Dequeue(StaticBody body)
	{
		foreach(var i in cars)
		{
			if (body.Filename == i.ScenePath)
			{
				i.Dismiss();
				Train.RemoveFirst();
				return;
			}
		}
	}
	
	public void Dequeue(Area body)
	{
		foreach(var j in breeds)
		{
			if (body.Filename == j.ScenePath)
			{
				j.Dismiss();
				Riders.RemoveFirst();
				return;
			}
		}
	}
	
	public void Clear()
	{
		foreach(var obj in cars) 
			obj.Clear();
		Train.Clear();
		
		foreach(var obj in breeds)
			obj.Clear();
		Riders.Clear();
	}
}
