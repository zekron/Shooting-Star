using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Audio Data SO")]
public class AudioDataSO : ScriptableObject
{
    public AudioClip[] audioClip;
    public float volume;
}
