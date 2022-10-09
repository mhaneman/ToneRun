using Godot;
using System;

public class SwipeDetector : Node
{
	[Signal] delegate void Swiped(string direction);
	private Vector2 start_pos;
	private bool can_detect = false;
	private float detect_radius = 50f;

	public override void _UnhandledInput(InputEvent @event) {
		if (@event is InputEventScreenTouch eventKey) 
		{
			if (eventKey.Pressed)
			{
				start_pos = eventKey.Position;
				can_detect = true;
			}
		}
	
	}
	
	public override void _Process(float delta)
	{
		/* for PC play */
		if (Input.IsActionJustPressed("left"))
			EmitSignal("Swiped", "left");
		if (Input.IsActionJustPressed("right"))
			EmitSignal("Swiped", "right");
		if (Input.IsActionJustPressed("jump"))
			EmitSignal("Swiped", "jump");
		if (Input.IsActionJustPressed("down"))
			EmitSignal("Swiped", "down");

		if (can_detect)
		{
			Vector2 cur_pos = GetViewport().GetMousePosition();
			float displ = start_pos.DistanceTo(cur_pos);
			
			if (displ >= detect_radius)
			{
				// then figure out what direction that is and send signal
				EndDetection(cur_pos);
				can_detect = false;	
			}	
		}
	}
	
	private void EndDetection(Vector2 end_pos) {
		float dis_x = start_pos.x - end_pos.x;
		float dis_y = start_pos.y - end_pos.y;
		
		if (dis_x != 0 && Mathf.Abs(dis_y)/Mathf.Abs(dis_x) < 1) 
		{
			if (dis_x > 0)
				EmitSignal("Swiped", "left");
			else
				EmitSignal("Swiped", "right");
		} 
		else 
		{
			if (dis_y > 0)
				EmitSignal("Swiped", "jump");
			else
				EmitSignal("Swiped", "down");
		}
	}
}
