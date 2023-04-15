using darcproducts;
using UnityEngine;

[System.Serializable]
public class AudioFXByLayer : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips = new AudioClip[0];
    [SerializeField] LayerMask[] audioLayers = new LayerMask[0];
    [SerializeField] AudioSource audioSource;

    public void PlayAudioFX(GameObject obj)
    {
        for (int i = 0; i < audioLayers.Length; i++)
        {
            if (Utilities.IsInLayerMask(obj, audioLayers[i]))
            {
                audioSource.Stop();
                audioSource.clip = audioClips[i];
                audioSource.Play();
                return;
            }
        }
    }
}
