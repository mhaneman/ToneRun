using Godot;
using System;

public class IntroCanvasOverlay : CanvasLayer
{
    private Resource UserSettings = new Resource();
    Button SettingsButton;
    Popup SettingsPopup;
    public override void _Ready()
    {
        SettingsPopup = GetNode<Popup>("SettingsPopup");
        SettingsPopup.Connect("popup_hide", this, "on_SettingsPopupHide");

        SettingsButton = GetNode<Button>("SettingsButton");
        SettingsButton.Connect("pressed", this, "on_SettingsButtonPressed");
    }

    private void on_SettingsButtonPressed()
    {
        SettingsPopup.Popup_();
    }

    private void on_SettingsPopupHide()
    {
        // save user data?
    }

    
}
