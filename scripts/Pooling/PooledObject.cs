using Godot;
using System;

using System.Collections.Generic;

public class PooledObject<T> where T : StaticBody
{
	private Transform discardLoc = new Transform();
	public LinkedList<T> working = new LinkedList<T>(); // eventually make double linked list
	private Stack<T> retired = new Stack<T>();
	
	private Spatial Other;
	public string ScenePath;
	
	public T PeekWorking()
	{
		if (working.Count > 0)
			return working.Last.Value;
		return null;
	}
	
	public PooledObject(Spatial Other, string ScenePath, int InitCount=1) 
	{
		this.discardLoc.origin = new Vector3(0f, 0f, 200f);
		this.Other = Other;
		this.ScenePath = ScenePath;

		DefferedInstance(InitCount); 
	}
	
	private void DefferedInstance(int count=1) 
	{
		for (int i=0; i<count; i++)
		{
			PackedScene scene = (PackedScene)ResourceLoader.Load(ScenePath);
			T t = scene.Instance<T>();
			Other.CallDeferred("add_child", t);
			t.GlobalTransform = discardLoc;
			t.Visible = false;
			retired.Push(t);
		}
	}

	private void RunningInstance(int count=1)
	{
		for (int i=0; i<count; i++)
		{
			PackedScene scene = (PackedScene)ResourceLoader.Load(ScenePath);
			T t = scene.Instance<T>();
			Other.AddChild(t);
			t.GlobalTransform = discardLoc;
			t.Visible = false;
			retired.Push(t);
		}

	}
	
	private T MakeActive() 
	{
		T t;
		if (retired.Count <= working.Count)
			RunningInstance(working.Count / 2);
		t = retired.Pop();
		t.Visible = true;
		return t;
	}
	
	public T Summon(Transform transform) {
		T t = MakeActive();
		t.GlobalTransform = transform;
		working.AddLast(t);

		return t;
	}

	public T Summon(Transform transform, Vector3 scale)
	{
		T t = MakeActive();
		t.GlobalTransform = transform;
		t.Scale = scale;
		working.AddLast(t);

		return t;
	}
	
	public void Dismiss() 
	{
		if (working.Count == 0)
			return;
		
		T t = working.First.Value;
		working.RemoveFirst();
		
		t.GlobalTransform = discardLoc;
		t.Visible = false;
		retired.Push(t);

		GD.Print("working: ", working.Count, " retired: ", 
			retired.Count, " ", ScenePath);
	}
	
	public void Clear()
	{
		
		while(working.Count > 0)
			this.Dismiss();
			
		/* GD.Print("working: ", working.Count, " retired: ", 
			retired.Count, " ", ScenePath); */
	}
}