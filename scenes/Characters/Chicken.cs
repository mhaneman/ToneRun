using Godot;
using System;

public class Chicken : Spatial
{
	AnimationPlayer anim;
	public override void _Ready() {
		anim = GetNode<AnimationPlayer>("AnimationPlayer");	
	}


	public override void _Process(float delta) {
		anim.Play("run", -1, 3.0f, false);
	}
}
