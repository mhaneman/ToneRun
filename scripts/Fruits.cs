using Godot;
using System;

public class Fruits : Area
{
	float rot_speed = 4f;
	int store_current_color;
	
	AudioStreamPlayer collected;
	private Singleton st;
	public override void _Ready() 
	{
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("ChangeColor", this, "_on_Changed_Color");
		
		collected = GetNode<AudioStreamPlayer>("Collected");
		
		store_current_color = (int) st.current_color;
		MeshInstance current_fruit = (MeshInstance) this.GetChild(store_current_color);
		current_fruit.Visible = true;
	}
	
	public override void _Process(float delta) 
	{
		this.RotateY(rot_speed * delta);
	}
	
	private void _on_Changed_Color()
	{
		MeshInstance store_fruit = (MeshInstance) this.GetChild(store_current_color);
		MeshInstance wanted_fruit = (MeshInstance) this.GetChild((int) st.current_color);
		store_fruit.Visible = false;
		wanted_fruit.Visible = true;
		store_current_color = (int) st.current_color;
	}
	
	private void _on_Fruit_body_entered(object body)
	{
		this.Visible = false;
		st.fruits++;
		collected.Play();
	}
}
