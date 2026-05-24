namespace EnragedV1.Classes;

using HarmonyLib;
using Mod;
using Mod.Classes;
using UnityEngine;

public class EnragedAudio : MonoBehaviour
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
            source.outputAudioMixerGroup = EnragedController.mutedGroup;
        else
            source.outputAudioMixerGroup = AudioMixerController.Instance.musicGroup;
    }
}

[HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), [])]
public class EnragedAudioPatch
{
    public static void Prefix(AudioSource __instance)
    {
        if (AudioMixerController.Instance == null) return;

        if (__instance.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && __instance.GetComponent<EnragedAudio>() == null && __instance.GetComponent<EnragedController>() == null)
            __instance.gameObject.AddComponent<EnragedAudio>();
    }
}

[HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), [typeof(double)])]
public class EnragedAudioPatch2
{
    public static void Prefix(AudioSource __instance)
    {
        if (AudioMixerController.Instance == null) return;

        if (__instance.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && __instance.GetComponent<EnragedAudio>() == null && __instance.GetComponent<EnragedController>() == null)
            __instance.gameObject.AddComponent<EnragedAudio>();
    }
}

[HarmonyPatch(typeof(AudioSource), nameof(AudioSource.PlayDelayed), [typeof(float)])]
public class EnragedAudioPatch23
{
    public static void Prefix(AudioSource __instance)
    {
        if (AudioMixerController.Instance == null) return;

        if (__instance.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && __instance.GetComponent<EnragedAudio>() == null && __instance.GetComponent<EnragedController>() == null)
            __instance.gameObject.AddComponent<EnragedAudio>();
    }
}

[HarmonyPatch(typeof(AudioSource), nameof(AudioSource.PlayScheduled), [typeof(float)])]
public class EnragedAudioPatch24
{
    public static void Prefix(AudioSource __instance)
    {
        if (AudioMixerController.Instance == null) return;

        if (__instance.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && __instance.GetComponent<EnragedAudio>() == null && __instance.GetComponent<EnragedController>() == null)
            __instance.gameObject.AddComponent<EnragedAudio>();
    }
}

[HarmonyPatch(typeof(AudioSource), nameof(AudioSource.Play), [typeof(ulong)])]
public class EnragedAudioPatch22
{
    public static void Prefix(AudioSource __instance)
    {
        if (AudioMixerController.Instance == null) return;

        if (__instance.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && __instance.GetComponent<EnragedAudio>() == null && __instance.GetComponent<EnragedController>() == null)
            __instance.gameObject.AddComponent<EnragedAudio>();
    }
}

[HarmonyPatch(typeof(AudioSource), nameof(AudioSource.UnPause))]
public class EnragedAudioPatch3
{
    public static void Prefix(AudioSource __instance)
    {
        if (AudioMixerController.Instance == null) return;

        if (__instance.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && __instance.GetComponent<EnragedAudio>() == null && __instance.GetComponent<EnragedController>() == null)
            __instance.gameObject.AddComponent<EnragedAudio>();
    }
}

[HarmonyPatch(typeof(AudioSource), nameof(AudioSource.PlayOneShot), [typeof(AudioClip)])]
public class EnragedAudioPatch4
{
    public static void Prefix(AudioSource __instance)
    {
        if (AudioMixerController.Instance == null) return;
        
        if (__instance.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && __instance.GetComponent<EnragedAudio>() == null && __instance.GetComponent<EnragedController>() == null)
            __instance.gameObject.AddComponent<EnragedAudio>();
    }
}

[HarmonyPatch(typeof(AudioSource), nameof(AudioSource.PlayOneShot), [typeof(AudioClip), typeof(float)])]
public class EnragedAudioPatch5
{
    public static void Prefix(AudioSource __instance)
    {
        if (AudioMixerController.Instance == null) return;

        if (__instance.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && __instance.GetComponent<EnragedAudio>() == null && __instance.GetComponent<EnragedController>() == null)
            __instance.gameObject.AddComponent<EnragedAudio>();
    }
}
