using Godot;
using System;
using System.Threading.Tasks;

public class World : Spatial
{	
	private Singleton st;

	public override void _Ready()
	{
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("PlayerDied", this, "on_PlayerDied");
	}
	
	private void _on_DeathBound_body_entered(object body)
	{
		st.EmitSignal("PlayerDied");
	}

	private void on_PlayerDied()
	{
		GetTree().ChangeScene("res://scenes/World/Intro.tscn");
	}
}
