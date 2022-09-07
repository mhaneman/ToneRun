using Godot;
using System;

public class CharacterSkin : Spatial
{
    AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void PlayAnimation(string anim, float speed)
    {
        animationPlayer.PlaybackSpeed = speed;
        animationPlayer.Play(anim);
    }
}
