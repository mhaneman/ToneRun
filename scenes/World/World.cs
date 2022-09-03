using Godot;
using System;

public class World : Spatial
{	
	private Singleton st;
	Random rand = new Random();
	public override void _Ready()
	{
		st = GetNode<Singleton>("/root/Singleton");
	}
	
	
	
	private void _on_DeathBound_body_entered(object body)
	{
		st.EmitSignal("PlayerDied");
	}
}
