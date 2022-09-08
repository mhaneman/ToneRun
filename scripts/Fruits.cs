using Godot;
using System;

public class Fruits : Area
{
	float rot_speed = 2f;
	int store_current_color;
	
	AudioStreamPlayer collected;
	private Singleton st;
	public override void _Ready() 
	{
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("ChangeColor", this, "on_ChangeColor");
		
		collected = GetNode<AudioStreamPlayer>("Collected");
		
		store_current_color = (int) st.CurrentColor;
		Spatial current_fruit = (Spatial) this.GetChild(store_current_color);
		current_fruit.Visible = true;
	}
	
	public override void _Process(float delta) 
	{
		this.RotateY(rot_speed * delta);
	}
	
	private void on_ChangeColor()
	{
		Spatial store_fruit = (Spatial) this.GetChild(store_current_color);
		Spatial wanted_fruit = (Spatial) this.GetChild((int) st.CurrentColor);
		store_fruit.Visible = false;
		wanted_fruit.Visible = true;
		store_current_color = (int) st.CurrentColor;
	}
	
	private void _on_Fruit_body_entered(object body)
	{
		this.Visible = false;
		st.fruits++;
		collected.Play();
	}
}
