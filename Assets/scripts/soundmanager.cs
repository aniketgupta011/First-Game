using System;
using UnityEngine;

public class soundmanager : MonoBehaviour
{
    private static int soundVolume = 6;
    public static soundmanager Instance { get; private set; }
    private const int soundVolumeMax = 10;
    public event EventHandler OnSoundVolumeChanged;

    [SerializeField] private AudioClip fuelPickAudioClip;
    [SerializeField]private AudioClip coinPickupAudioClip;
    [SerializeField] private AudioClip crashAudioClip;
    [SerializeField] private AudioClip landingSuccessAudioClip;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        lander.Instance.OnLanded += Lander_OnLanded;
    }
    private void Lander_OnLanded(object sender, lander.OnLandedEventArgs e)
    {
        switch(e.landingType)
        {
            case lander.LandingType.Success:
                AudioSource.PlayClipAtPoint(landingSuccessAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
                break;
            default: AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
                break;
        }
    }
    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }
    private void Lander_OnFuelPickup(object sender, System.EventArgs e)
    { AudioSource.PlayClipAtPoint(fuelPickAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized()); }

    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % soundVolumeMax;
       OnSoundVolumeChanged?.Invoke(this,EventArgs.Empty);
    }
    public int GetSoundVolume()
    { return soundVolume; }

    public float GetSoundVolumeNormalized()
    {
        return ((float)soundVolume) / soundVolumeMax;
    }
}
 
