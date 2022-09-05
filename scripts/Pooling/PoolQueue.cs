using Godot;
using System;

using System.Collections.Generic;

public class PoolQueue<T> : Node where T : StaticBody
{
	private int NumOfLanes;
	public T[] Head;
	private Dictionary<string, PooledObject<T>> ObjTypes = new Dictionary<string, PooledObject<T>>();
	private Spatial Other;
	
	public PoolQueue(){}
	
	public PoolQueue(Spatial Other, int NumOfLanes)
	{
		this.Other = Other;
		this.NumOfLanes = NumOfLanes;
		Head = new T[NumOfLanes];
	}
	
	public void AddObjType(string Name, string ScenePath, int InitCount=1)
	{
		ObjTypes.Add(Name, new PooledObject<T>(Other, ScenePath, InitCount));
	}

	public void InitalizePlatforms((string ObjName, Vector3 Scale, float offset)[] Platforms)
	{
		for(int n=0; n<Platforms.Length; n++)
		{
			Transform trans = Transform.Identity;
			trans.origin.x = Platforms[n].offset;
			Head[n] = ObjTypes[Platforms[n].ObjName].Summon(trans, Platforms[n].Scale);
		}
	}
	
	
	public void Enqueue((string ObjName, Vector3 Scale)[] Platform)
	{
		T[] NewHead = new T[NumOfLanes];
		for (int n=0; n<Platform.Length; n++)
		{
			Transform SpawnLoc = Head[n].GetNode<Spatial>("Back").GlobalTransform;
			NewHead[n] = ObjTypes[Platform[n].ObjName].Summon(SpawnLoc, Platform[n].Scale);
		}
		Head = NewHead;
	}
	
	public void Dequeue(T body)
	{
		foreach(var i in ObjTypes.Values)
		{
			if (body.Filename == i.ScenePath)
			{
				i.Dismiss();
				return;
			}
		}
	}
	
	public float GetHeight(int row)
	{
		return Head[row].GetNode<Spatial>("Back").GlobalTransform.origin.y;
	}

	public void MoveObjects(float Speed, float delta)
	{
		foreach(var i in ObjTypes.Values)
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
