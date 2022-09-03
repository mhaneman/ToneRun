using Godot;
using System;
using System.Collections.Generic; 

public class Treadmill : Spatial 
{
	Spatial BeltLeft;
	Spatial BeltMiddle;
	Spatial BeltRight;
	
	
	private Singleton st;
	public override void _Ready()
	{	
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("ChangeColor", this, "_on_change_color");
		
		BeltLeft = GetNode<Spatial>("BeltLeft");
		BeltMiddle = GetNode<Spatial>("BeltMiddle");
		BeltRight = GetNode<Spatial>("BeltRight");
		
		BeltLeft.Call("Init", st.speed);
		BeltMiddle.Call("Init", st.speed);
		BeltRight.Call("Init", st.speed);
	}
	
	private void _on_change_color()
	{
		if (st.current_color == (int) Colors.Black)
		{
			st.speed += 20f;
			BeltLeft.Call("SetSpeed", st.speed);
			BeltMiddle.Call("SetSpeed", st.speed);
			BeltRight.Call("SetSpeed", st.speed);
		}
	}
}
