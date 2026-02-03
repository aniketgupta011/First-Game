using System;
using UnityEngine;

public class Musicmanager : MonoBehaviour
{
    private AudioSource musicAudioSource;
    private static float musicTime;
    private static int musicVolume = 4;
    private const int musicVolumeMax = 10;
    public event EventHandler OnMusicVolumeChanged;
    public static Musicmanager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
    }

    private void Start()
    {
        musicAudioSource.volume = GetMusicVolumeNormalized();
    }

    private void Update()
    {
        musicTime = musicAudioSource.time;
    }
    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % musicVolumeMax;
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
    public int GetMusicVolume()
    { return musicVolume; }

    public float GetMusicVolumeNormalized()
    {
        return ((float)musicVolume) / musicVolumeMax;
    }
}
