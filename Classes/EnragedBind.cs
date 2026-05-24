namespace Mod.Classes;

using EnragedV1.Classes;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EnragedV1.CustomBindingsPoCPlugin;

[CreateOnStart]
public class EnragedBind : MonoBehaviour
{
    public void Awake() => SceneManager.sceneLoaded += OnSceneLoad;

    public void OnSceneLoad(Scene _, LoadSceneMode __) => Invoke(nameof(LoadBinds), 0.1f);

    public void LoadBinds()
    {
        Plugin.config.Reload();

        if (SceneHelper.CurrentScene is "Intro" or "Bootstrap")
            return;

        foreach (var item in FindObjectsOfType<AudioSource>(true))
        {
            if (item.outputAudioMixerGroup == AudioMixerController.Instance.musicGroup && item.GetComponent<EnragedAudio>() == null && item.GetComponent<EnragedController>() == null)
            {
                item.gameObject.AddComponent<EnragedAudio>();
            }
        }

        foreach (var item in FindObjectsOfType<GetMusicVolume>(true))
        {
            if (item.GetComponent<EnragedAudioVolume>() == null)
            {
                item.gameObject.AddComponent<EnragedAudioVolume>();
            }
        }

        if (FindObjectOfType<TestListener>() == null)
        {
            GameObject listener = new GameObject("EnragedInputListener");
            DontDestroyOnLoad(listener);
            listener.AddComponent<TestListener>();
        }
    }

    public static void Press()
    {
        if (OptionsManager.instance == null || OptionsManager.instance.paused || NewMovement.Instance == null || (!NewMovement.Instance.isActiveAndEnabled && (PlatformerMovement.Instance == null || !PlatformerMovement.instance.isActiveAndEnabled))) return;

        EnragedController.instance.ToggleEnrage();
    }
}