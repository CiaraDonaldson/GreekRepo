using UnityEngine;

public sealed class AudioFX : MonoBehaviour
{
    enum PlayType
    {
        PlayOneShot,
        StopThenPlay,
        PlayIfNotPlaying
    }

    [SerializeField] private PlayType currentPlayType;
    [SerializeField] private bool useSource = true;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Vector2 minMaxVolume;
    [SerializeField] private Vector2 minMaxPitch;

    public void PlayFX() => PlayFX(null);

    public void PlayFXWithChance(float chance)
    {
        if (Random.value < Mathf.Clamp01(chance))
            PlayFX();
    }

    public void PlayFX(GameObject target)
    {
        AudioClip clipToPlay = clips[Random.Range(0, clips.Length)];
        if (clipToPlay == null) return;
        if (source != null)
        {
            source.volume = Mathf.Clamp01(Random.Range(Mathf.Min(minMaxVolume.x, minMaxVolume.y), Mathf.Max(minMaxVolume.x, minMaxVolume.y)));
            source.pitch = Mathf.Clamp(Random.Range(Mathf.Min(minMaxPitch.x, minMaxPitch.y), Mathf.Max(minMaxPitch.x, minMaxPitch.y)), -3, 3);
        }
        switch (currentPlayType)
        {
            case PlayType.PlayOneShot:
                if (!useSource && target != null)
                    AudioSource.PlayClipAtPoint(clipToPlay, target.transform.position, Random.Range(minMaxVolume.x, minMaxVolume.y));
                else if (source != null)
                    source.PlayOneShot(clipToPlay);
                break;

            case PlayType.StopThenPlay:
                source.Stop();
                source.clip = clipToPlay;
                source.Play();
                break;

            case PlayType.PlayIfNotPlaying:
                if (!source.isPlaying)
                {
                    source.clip = clipToPlay;
                    source.Play();
                }
                break;
        }
    }
}