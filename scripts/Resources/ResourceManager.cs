using Godot;
using System;

public class ResourceManager
{

    private static String platformShaderFlatLoc = "res://resources/PlatformColorFlat.tres";
	private static String platformShaderStairLoc = "res://resources/PlatformColorStair.tres";
	private static String ProcSkyLoc = "res://resources/ProcSky.tres";

    public ShaderMaterial platformShaderFlat = (ShaderMaterial) GD.Load(platformShaderFlatLoc);
	public ShaderMaterial platformShaderStair = (ShaderMaterial) GD.Load(platformShaderStairLoc);
	public ProceduralSky ProcSky = (ProceduralSky) GD.Load(ProcSkyLoc);

    public ResourceManager(int InitalColor=0)
    {
        ChangeTones(InitalColor);
    }

    public void ChangeTones(int tone)
	{
		platformShaderFlat.SetShaderParam("albedo", FlatColors[tone]);
		platformShaderStair.SetShaderParam("albedo", StairColors[tone]);

		// ProcSky.SkyHorizonColor = SkyHorizionColors[tone];
		// ProcSky.GroundHorizonColor = SkyHorizionColors[tone];

		ResourceSaver.Save(platformShaderFlatLoc, platformShaderFlat, 0);
		ResourceSaver.Save(platformShaderStairLoc, platformShaderStair, 0);
		ResourceSaver.Save(ProcSkyLoc, ProcSky, 0);
	}

	// low contrast
	private Color[] FlatColors =
	{
		new Color("#352c2e"), // strawberry
		new Color("#494639"), // orange
		new Color("#a5a27d"), // banana
		new Color("#ecf4c8"), // pear
		new Color("#211f3d"), // blueberry
		new Color("#281e2b"), // eggplant
		new Color("#141414"), // special
	};

	// high contrast
	private Color[] StairColors =
	{
		new Color("#e5bee8"),
		new Color("#ffb7b7"),
		new Color("#ffde93"),
		new Color("#42593b"),
		new Color("#7aadd3"),
		new Color("#e5c6f4"),
		new Color("#ffffff"),
	};
}
