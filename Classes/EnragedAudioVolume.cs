namespace EnragedV1.Classes;

using Mod;
using Mod.Classes;
using UnityEngine;

public class EnragedAudioVolume : MonoBehaviour
{
    public AudioSource source;

    public float startVolume;

    public void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        startVolume = source.volume;
    }

    public void Update()
    {
        if (Plugin.customMusicClip != null && EnragedController.MusicPlaying())
            source.volume = 0;
    }
}