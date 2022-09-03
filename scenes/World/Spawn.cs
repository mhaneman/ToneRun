using Godot;
using System;

public class Spawn : Area
{
	[Signal] delegate void LowSpawn();
	
	public override void _PhysicsProcess(float delta)
	{
		if (this.GetOverlappingBodies().Count == 0) {
			EmitSignal("LowSpawn");
		}
	}
}
