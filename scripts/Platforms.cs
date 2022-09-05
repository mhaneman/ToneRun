using Godot;
using System;

public class Platforms : Spatial
{
    private float SwitchSpeed = 30f;
    private float NumOfLanes;
    private float PlatformSpacing;

    private int WantedLane;

    private Singleton st;
    public override void _Ready()
    {
        st = GetNode<Singleton>("/root/Singleton");
		st.Connect("PlayerDied", this, "on_PlayerDied");
		this.NumOfLanes = st.NumOfLanes;
		this.PlatformSpacing = st.PlatformSpacing;
        this.WantedLane = 0;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Transform trans = this.GlobalTransform;
        trans.origin.x = Mathf.Lerp(trans.origin.x, WantedLane * PlatformSpacing, 0.2f);
        this.GlobalTransform = trans;
    }

    private void on_Swiped(string direction)
    {
        if (direction == "left")
        {
            WantedLane++;
        }

        if (direction == "right")
        {
            WantedLane--;
        }
    }
}
