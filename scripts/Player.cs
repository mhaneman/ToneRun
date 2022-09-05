using Godot;
using System;

public class Player : KinematicBody
{
	float switch_speed = 30f;
	float gravity = 1.3f;
	float jump_power = 35f;
	float max_terminal_velocity = 200f;
	
	private Spatial camera_pivot;
	private Camera camera;
	
	private Vector3 velocity;
	
	private int current_lane;
	private int wanted_lane;
	private float PlatformSpacing;
	
	AudioStreamPlayer Switch;
	AudioStreamPlayer Jump;
	AudioStreamPlayer Death;
	
	private Singleton st;
	public override void _Ready()
	{
		
		/* signals */
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("PlayerDied", this, "_on_Player_Died");
		this.PlatformSpacing = st.PlatformSpacing;
		
		/* audio */
		Death = GetNode<AudioStreamPlayer>("Death");
		Jump = GetNode<AudioStreamPlayer>("Jump");
		Switch = GetNode<AudioStreamPlayer>("Switch");
		
		camera_pivot = GetNode<Spatial>("CameraPivot");
		camera = GetNode<Camera>("CameraPivot/CameraBoom/Camera");
		
		
		current_lane = wanted_lane = 0;
	}
	
	// touch screen controls
	private void _on_SwipeDetector_Swiped(string direction)
	{
		if (direction == "left") 
		{
			wanted_lane -= 1;
		}
		
		if (direction == "right") 
		{
			wanted_lane += 1;
		}
		
		if (direction == "jump" && IsOnFloor())
		{
			velocity.y = jump_power;
			Jump.Play();
		}
		
		if (direction == "down" && !IsOnFloor())
		{
			GD.Print("fasty fall");
			velocity.y = -jump_power;
		}
	}
	
	public override void _UnhandledInput(InputEvent @event) {
		if (Input.IsActionJustPressed("jump") && IsOnFloor()) 
		{
			velocity.y += jump_power;
			Jump.Play();	
		}
		
		if (Input.IsActionJustPressed("down") && !IsOnFloor())
		{
			velocity.y += -jump_power;
		}
		
		if (Input.IsActionJustPressed("left")) 
		{
			wanted_lane -= 1;
		}
		
		if (Input.IsActionJustPressed("right")) 
		{
			wanted_lane += 1;
		}
		
	}

	private void _on_Player_Died()
	{
		Transform respawn = this.GlobalTransform;
		respawn.origin.y = 60;
		this.GlobalTransform = respawn;
		Death.Play();
	}
	
	public override void _PhysicsProcess(float delta) 
	{
		base._PhysicsProcess(delta);
		control_physics(delta);
		gravity_physics(delta);
	}
	
	private void control_physics(float delta) 
	{
		if (current_lane > wanted_lane)
			switch_lane(delta, "right");
		else if (current_lane < wanted_lane)
			switch_lane(delta, "left");
	}
	
	private void switch_lane(float delta, string direction) {
		Switch.Play();
		// Im not gonna remember if a signed integer is right or left ...
		int dir;
		if (direction == "left")
			dir = 1;
		else if (direction == "right")
			dir = -1;
		else
			return;
		
		float wanted_x = wanted_lane * PlatformSpacing;
		float current_x = GlobalTransform.origin.x;
		float diff_x = Mathf.Abs(current_x - wanted_x);
		Transform t = this.Transform;
		
		if (Mathf.Abs(delta * switch_speed) >= diff_x) 
		{
			t.origin.x = wanted_x;
			current_lane = wanted_lane;
		} 
		else
			t.origin.x += switch_speed * delta * dir;
		this.Transform = t;
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
}
