using System.Collections;
using System.Collections.Generic;
using Ebac.Core.Singleton;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource audioSource;
    private AudioClip _audioClip;

    public void playSound(float volume)
    {
        audioSource.clip = _audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void setSound(AudioClip audioClip)
    {
        _audioClip = audioClip;
    }
}
