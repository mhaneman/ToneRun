using Godot;
using System;

public class GlobalSettings : CanvasLayer
{
    string store_file = "user://data.save";
    string settings;

    public override void _Ready()
    {
        settings = LoadSettings();
    }

    private string LoadSettings()
    {
        var f = new File();
        if (f.FileExists(store_file))
        {
            f.Open(store_file, File.ModeFlags.Read);
            var content = f.GetAsText();
            f.Close();
            return content;
        }
        return "";
    }

    private void StoreSettings()
    {
        var f = new File();
        f.Open(store_file, File.ModeFlags.Write);
        f.StoreString("");
        f.Close();
        
    }
}
