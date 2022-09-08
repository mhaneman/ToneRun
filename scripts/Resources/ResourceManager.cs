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

		ProcSky.SkyHorizonColor = ProcSky.GroundHorizonColor = SkyColors[tone];

		ResourceSaver.Save(platformShaderFlatLoc, platformShaderFlat, 0);
		ResourceSaver.Save(platformShaderStairLoc, platformShaderStair, 0);
		ResourceSaver.Save(ProcSkyLoc, ProcSky, 0);
	}

	private Color[] FlatColors =
	{
		new Color("#15EA62"),
		new Color("#BC11EE"),
		new Color("#0CF3F3"),
		new Color("#0152FE"),
		new Color("#FF5300"),
		new Color("#FFEB00"),
		new Color("#040A0B"),
	};

	private Color[] StairColors =
	{
		new Color("#6215EA"),
		new Color("#11EEBC"),
		new Color("#F30CF3"),
		new Color("#FE0152"),
		new Color("#00FF53"),
		new Color("#00FFEB"),
		new Color("#1DAEE2"),
	};

	private Color[] SkyColors =
	{
		new Color("#6215EA"),
		new Color("#11EEBC"),
		new Color("#F30CF3"),
		new Color("#FE0152"),
		new Color("#00FF53"),
		new Color("#00FFEB"),
		new Color("#1DAEE2"),
	};
}
