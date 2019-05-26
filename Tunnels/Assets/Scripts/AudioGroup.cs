using UnityEngine;

[CreateAssetMenu(fileName = "Audio Group", menuName = "Audio Group", order = 4)]
public class AudioGroup : ScriptableObject
{
    public AudioClip[] clips;

    public AudioClip GetRandom()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
