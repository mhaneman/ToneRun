using Godot;
using System;

using System.Collections.Generic;

public class Music : Node
{	
	List<AudioStreamPlayer> audio = new List<AudioStreamPlayer>();
		
	private Singleton st;
	public override void _Ready()
	{
		st = GetNode<Singleton>("/root/Singleton");
		
		foreach(AudioStreamPlayer i in this.GetChildren())
		{
			i.Connect("finished", this, "on_Finished");
			audio.Add(i);	
		}
		audio[st.CurrentColor].Play();
	}
	
	private void on_Finished()
	{
		st.EmitSignal("ChangeColor");
		audio[st.CurrentColor].Play();
		
	}
}
