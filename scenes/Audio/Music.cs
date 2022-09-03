using Godot;
using System;

using System.Collections.Generic;

public class Music : Node
{	List<AudioStreamPlayer> audio = new List<AudioStreamPlayer>();
	WeightedRandom colors = new WeightedRandom(7);
		
	private Singleton st;
	public override void _Ready()
	{
		st = GetNode<Singleton>("/root/Singleton");
		
		foreach(AudioStreamPlayer i in this.GetChildren())
		{
			i.Connect("finished", this, "on_Finished");
			audio.Add(i);	
		}
		audio[st.current_color].Play();
	}
	
	private void on_Finished()
	{
		ChangeColor();
	}
	
	private void ChangeColor()
	{
		st.current_color = colors.WeightedRandomIndex();
		colors.ShiftWeight(st.current_color, 0f);
		st.EmitSignal("ChangeColor");
		audio[st.current_color].Play();
	}
	
}
