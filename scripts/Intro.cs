using Godot;
using System;

public class Intro : Spatial
{
	private Node ThrowDetector;
	private RigidBody Player;
	private Camera camera;
	private Area area;
	
	private PackedScene mainGameScene;

	public override void _Ready()
	{
		camera = GetNode<Camera>("Camera");
		Player = GetNode<RigidBody>("Player");
		area = GetNode<Area>("Area");

		ThrowDetector = GetNode<Node>("ThrowDetector");
		ThrowDetector.Connect("Throw", this, "on_Throw");

		area.Connect("body_exited", this, "on_BodyExited");
		
		mainGameScene = GD.Load<PackedScene>("res://scenes/World/World.tscn");
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		camera.LookAt(Player.GlobalTransform.origin, Vector3.Up);
	}

	private void on_Throw(Vector2 direction)
	{
		Vector3 pos = new Vector3 (0.1f, -0.1f, 0f);
		Vector3 impulse = new Vector3(0f, direction.y, direction.x) * 100f;
		Player.ApplyImpulse(pos, impulse);
	}

	private void on_BodyExited(object body)
	{
		GetTree().ChangeSceneTo(mainGameScene);
	}
}
