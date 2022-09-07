using Godot;
using System;

public class Player : KinematicBody
{
	private float gravity = 1.3f;
	private float jump_power = 40f;
	private float max_terminal_velocity = 200f;
	
	private Spatial camera_pivot;
	private Camera camera;
	
	private Vector3 velocity;
	
	AudioStreamPlayer Switch;
	AudioStreamPlayer Jump;
	AudioStreamPlayer Death;

	private CharacterSkin skin;
	
	private Singleton st;
	public override void _Ready()
	{
		
		/* signals */
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("PlayerDied", this, "on_PlayerDied");
		
		/* audio */
		Death = GetNode<AudioStreamPlayer>("Death");
		Jump = GetNode<AudioStreamPlayer>("Jump");
		Switch = GetNode<AudioStreamPlayer>("Switch");
		
		camera_pivot = GetNode<Spatial>("CameraPivot");
		camera = GetNode<Camera>("CameraPivot/CameraBoom/Camera");

		skin = GetNode<CharacterSkin>("chicken");
		skin.PlayAnimation("running-loop", 3f);
	}
	
	public override void _PhysicsProcess(float delta) 
	{
		base._PhysicsProcess(delta);
		gravity_physics(delta);
	}
	
	private void gravity_physics(float delta) {	
		if (IsOnWall())
		{
			Death.Play();
			st.EmitSignal("PlayerDied");
		}

		velocity.y = Mathf.Clamp(velocity.y-gravity, -max_terminal_velocity, jump_power);
		velocity = MoveAndSlide(velocity, Vector3.Up, false, 4, 1.5605f, false);
	}

	/* ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ */
	/* Signals */
	
	private void _on_SwipeDetector_Swiped(string direction)
	{
		if (direction == "jump" && IsOnFloor())
		{
			velocity.y = jump_power;
			Jump.Play();
		}
		
		if (direction == "down" && !IsOnFloor())
		{
			velocity.y = -jump_power;
		}

		if (direction == "left")
		{
		}

		if (direction == "right")
		{
		}
	}

	private void on_PlayerDied()
	{
		Death.Play();
	}
}
