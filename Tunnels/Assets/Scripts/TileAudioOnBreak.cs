using UnityEngine;

public class TileAudioOnBreak : MonoBehaviour
{
    [SerializeField]
    private GameObject onBreakSoundPrefab;

    [SerializeField]
    private AudioGroup onBreakSounds;

    void OnTileBreak(DrillAttackInfo info)
    {
        GameObject onBreakObject = Instantiate(onBreakSoundPrefab, transform.position, Quaternion.identity);
        AudioSource source = onBreakObject.GetComponent<AudioSource>();
        AudioClip clip = onBreakSounds.GetRandom();
        source.PlayOneShot(clip);
        Destroy(onBreakObject, clip.length);
    }
}
