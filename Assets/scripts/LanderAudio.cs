using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioScource;
    private lander lander;

    private void Awake()
    {
        lander = GetComponent<lander>();
    }
    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        soundmanager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;
        thrusterAudioScource.Pause();
    }
    private void SoundManager_OnSoundVolumeChanged(object sender, System.EventArgs e)
    {
        thrusterAudioScource.volume = soundmanager.Instance.GetSoundVolumeNormalized();
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        if (!thrusterAudioScource.isPlaying)
        {
            thrusterAudioScource.Play();
        }
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        if (!thrusterAudioScource.isPlaying)
        {
            thrusterAudioScource.Play();
        }
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        if (!thrusterAudioScource.isPlaying)
        {
            thrusterAudioScource.Play();
        }
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        thrusterAudioScource.Pause();
    }
}
