using Godot;
using System;
using Godot.Collections;

public class WeightedRandom
{
	Random rand = new Random();
	
	Array<float> weights = new Array<float>();
	
	public WeightedRandom() {}
	public WeightedRandom(int count=0)
	{
		for (int i=0; i<count; i++)
			weights.Add(1f);
		
		NormalizeWeights();
	}
	
	private float Sum()
	{
		float result = 0f;
		foreach(float i in weights)
			result += i;
		return result;
	}
	
	private void EvenDist()
	{
		float dist = 1f / (float) weights.Count;
		for (int i=0; i<weights.Count; i++)
			weights[i] = dist;
	}
	
	private void NormalizeWeights()
	{
		float total_sum = Sum();
		if (total_sum < 0.001)
		{
			EvenDist();
			return;
		}
			
		for(int i=0; i < weights.Count; i++)
			weights[i] /= total_sum;
	}
	
	public int WeightedRandomIndex()
	{
		float rnd = (float) rand.NextDouble();
		float rolling_sum = 0f;
		for (int i=0; i<weights.Count; i++)
		{
			if (rnd <= weights[i] + rolling_sum)
				return i;
			rolling_sum += weights[i];
		}
		return 0;
	}
	
	public void ShiftWeight(int index, float weight)
	{
		weights[index] *= weight;
		NormalizeWeights();
	}
}
