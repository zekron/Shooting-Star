using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [Header("Components")]
    [SerializeField] private AudioSource sFXPlayer;
    [SerializeField] private AudioSource bGMPlayer;

    [Header("Events")]
    [SerializeField] private FloatEventChannelSO changeSFXVolumeEventSO;
    [SerializeField] private FloatEventChannelSO changeBGMVolumeEventSO;

    private const float MIN_PITCH = 0.9f;
    private const float MAX_PITCH = 1.1f;

    private void OnEnable()
    {
        changeSFXVolumeEventSO.OnEventRaised += SetSFXVolume;
        changeBGMVolumeEventSO.OnEventRaised += SetBGMVolume;
    }

    private void OnDisable()
    {
        changeSFXVolumeEventSO.OnEventRaised -= SetSFXVolume;
        changeBGMVolumeEventSO.OnEventRaised -= SetBGMVolume;
    }

    private void SetSFXVolume(float value)
    {
        sFXPlayer.volume = value;
    }
    private void SetBGMVolume(float value)
    {
        bGMPlayer.volume = value;
    }

    public void PlaySFX(AudioDataSO audioData)
    {
        if (audioData.audioClip.Length > 1)
        {
            PlayRandomClipSFX(audioData);
        }
        else
        {
            PlayRandomPitchSFX(audioData.audioClip[0], audioData.volume);
        }
    }

    public void PlayBGM(AudioDataSO audioData)
    {
        bGMPlayer.clip = audioData.audioClip[0];
        bGMPlayer.Play();
    }

    // Used for UI SFX
    private void PlayNormalPitchSFX(AudioClip clip, float volume)
    {
        sFXPlayer.pitch = 1;
        sFXPlayer.PlayOneShot(clip, volume);
    }

    private void PlayRandomPitchSFX(AudioClip clip, float volume)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        sFXPlayer.PlayOneShot(clip, volume);
    }

    // Used for repeat-play SFX
    private void PlayRandomClipSFX(AudioDataSO audioData)
    {
        PlayNormalPitchSFX(audioData.audioClip[Random.Range(0, audioData.audioClip.Length)], audioData.volume);
    }

    internal void PlaySFX(object dropInventorySFX)
    {
        throw new System.NotImplementedException();
    }
}