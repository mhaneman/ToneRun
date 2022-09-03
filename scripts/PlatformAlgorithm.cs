using Godot;
using System;

public class PlatformAlgorithm
{
	WeightedRandom weighted_platform = new WeightedRandom();
	WeightedRandom weighted_stair = new WeightedRandom();
	
	private int current_height = 1;
	
	private float[] ScaleXWeights = {1f, 2f, 1f};
	private float[] ScaleYWeights = {1f, 2f, 1f};
	private float[] ScaleYZWeights = {1f, 3f, 2f};
	
	
	public PlatformAlgorithm()
	{

	}
}
