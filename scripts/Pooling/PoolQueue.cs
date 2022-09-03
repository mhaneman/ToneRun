using Godot;
using System;

using System.Collections.Generic;

public class PoolQueue<T> : Node where T : StaticBody
{
	/* seperate the object pool out... eventually */
	private T[] Head = new T[3];
	private List<PooledObject<T>> ObjTypes = new List<PooledObject<T>>();
	private Spatial Other;
	private Vector3 Speed = Vector3.Back * 50f;
	
	public PoolQueue(){}
	
	public PoolQueue(Spatial Other)
	{
		this.Other = Other;
	}
	
	public void AddObjType(string ScenePath, int InitCount=1)
	{
		ObjTypes.Add(new PooledObject<T>(Other, ScenePath, InitCount));
	}

	public void InitalizePlatforms((int ObjNum, Vector3 Scale, float offset)[] Platforms)
	{
		for(int n=0; n<Platforms.Length; n++)
		{
			Transform trans = Transform.Identity;
			trans.origin.x = Platforms[n].offset;
			Head[n] = ObjTypes[Platforms[n].ObjNum].Summon(trans, Platforms[n].Scale);
		}
	}
	
	
	public void Enqueue((int ObjNum, Vector3 Scale)[] Platform)
	{
		T[] NewHead = new T[3];
		for (int n=0; n<Platform.Length; n++)
		{
			Transform SpawnLoc = Head[n].GetNode<Spatial>("end").GlobalTransform;
			NewHead[n] = ObjTypes[Platform[n].ObjNum].Summon(SpawnLoc, Platform[n].Scale);
		}
		Head = NewHead;
	}
	
	public void Dequeue(T body)
	{
		foreach(var i in ObjTypes)
		{
			if (body.Filename == i.ScenePath)
			{
				i.Dismiss();
				return;
			}
		}
	}

	public void MoveObjects(float Speed, float delta)
	{
		foreach(var i in ObjTypes)
		{
			foreach(var j in i.working)
			{
				Transform trans = j.GlobalTransform;
				trans.origin.z += Speed * delta;
				j.GlobalTransform = trans;
			}
		}
	}
}
