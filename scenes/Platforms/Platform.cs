using Godot;
using System;

public class Platform : StaticBody
{
	private Color[] FruitColors = {
		new Color("#ef4721"), // red
		new Color("#ef7721"), // orange
		new Color("#f4edc1"), // yellow
		new Color("#55ef21"), // green
		new Color("#232bc4"), // blue
		new Color("#7e21ef"), // purple
		new Color("#000000"), // black
	};
	
	private Singleton st;
	public override void _Ready() 
	{
		st = GetNode<Singleton>("/root/Singleton");
		st.Connect("ChangeColor", this, "_on_Changed_Color");
		ColorSwap();
	}
	
	private void _on_Changed_Color()
	{
		ColorSwap();
	}
	
	private void ColorSwap()
	{
		MeshInstance mesh = (MeshInstance) this.GetChild(0);
		SpatialMaterial material = (SpatialMaterial) mesh.GetSurfaceMaterial(0);
		material.AlbedoColor = FruitColors[(int) st.current_color];
		mesh.SetSurfaceMaterial(0, material);
	}
}
