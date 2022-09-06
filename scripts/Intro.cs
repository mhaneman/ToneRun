using Godot;
using System;

public class Intro : Spatial
{
    private Node SwipeDetector;
    private RigidBody Player;
    private Camera camera;
    private Area area;
    public override void _Ready()
    {
        camera = GetNode<Camera>("Camera");
        Player = GetNode<RigidBody>("Player");
        area = GetNode<Area>("Area");

        SwipeDetector = GetNode<Node>("SwipeDetector");
        SwipeDetector.Connect("Swiped", this, "on_Swiped");

        area.Connect("body_entered", this, "on_BodyEntered");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        camera.LookAt(Player.GlobalTransform.origin, Vector3.Up);
    }

    private void on_Swiped(string direction)
    {
        if (direction == "right")
            Player.ApplyImpulse(new Vector3(0.1f, -0.1f, 0f), new Vector3(0f, 20f, -70f));

         if (direction == "jump")
            Player.ApplyImpulse(new Vector3(0.1f, -0.1f, 0f), new Vector3(0f, 40f, 0f));
    }

    private void on_BodyEntered(object body)
    {
        GetTree().ChangeScene("res://scenes/World/World.tscn");
    }
}
