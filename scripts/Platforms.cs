using Godot;
using System;

public class Platforms : Spatial
{
    private float NumOfLanes;
    private float PlatformSpacing;
    private int WantedLane;


    private Belt belt;
    private Singleton st;

    public override void _Ready()
    {
        belt = GetNode<Belt>("Belt");
        st = GetNode<Singleton>("/root/Singleton");
		st.Connect("PlayerDied", this, "on_PlayerDied");

		this.NumOfLanes = st.NumOfLanes;
		this.PlatformSpacing = st.PlatformSpacing;
        this.WantedLane = 0;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        PlatformTransformLerp();
    }

    // moves the platforms to look like payer is changing lanes
    private void PlatformTransformLerp()
    {
        Transform trans = this.GlobalTransform;
        trans.origin.x = Mathf.Lerp(trans.origin.x, WantedLane * PlatformSpacing, 0.2f);
        this.GlobalTransform = trans;
    }

    /* ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ */
    /* Signals */

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

    private void on_PlayerDied()
    {
        belt.Speed = 0f;
    }
}
