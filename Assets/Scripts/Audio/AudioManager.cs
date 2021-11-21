using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] private AudioSource sFXPlayer;
    [SerializeField] private AudioSource bGMPlayer;

    private const float MIN_PITCH = 0.9f;
    private const float MAX_PITCH = 1.1f;

    public void PlaySFX(AudioDataSO audioData)
    {
        if (audioData.audioClip.Length > 1)
        {
            PlayRandomSFX(audioData);
        }
        else
        {
            PlaySFX(audioData.audioClip[0], audioData.volume);
        }
    }

    public void PlayBGM(AudioDataSO audioData)
    {
        bGMPlayer.clip = audioData.audioClip[0];
        bGMPlayer.Play();
    }

    // Used for UI SFX
    private void PlaySFX(AudioClip clip, float volume)
    {
        sFXPlayer.PlayOneShot(clip, volume);
    }

    // Used for repeat-play SFX
    private void PlayRandomSFX(AudioDataSO audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData.audioClip[Random.Range(0, audioData.audioClip.Length)], audioData.volume);
    }
}