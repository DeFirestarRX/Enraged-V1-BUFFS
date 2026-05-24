namespace Mod.Classes;

using EnragedV1.Classes;
using Helpers;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateOnStart]
public class EnragedController : MonoBehaviour
{
    private float originalWalkSpeed;
    private bool originalSpeedCaptured;

    private float originalHp;
    private bool originalHpCaptured;
    public static EnragedController instance;

    public static AudioMixerGroup mutedGroup;

    public static bool isEnraged = false;

    public GameObject canvas;
    public Image enragedImg, fadeImg;
    public FadeOutAnim fadeOutAnim;

    public AudioSource source, music;

    public GameObject rageEffect;

    public void Start()
    {
        instance = this;
        canvas = Instantiate(BundleLoader.bundle.LoadAsset<GameObject>("EnrageCanvas"));
        DontDestroyOnLoad(canvas);

        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;

        music = gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;

        enragedImg = canvas.transform.Find("Enrage").GetComponent<Image>();
        fadeImg = canvas.transform.Find("Animation").GetComponent<Image>();

        enragedImg.sprite = Plugin.Ass<Sprite>("Assets/Textures/UI/powerupvignette.png");
        enragedImg.type = Image.Type.Sliced;
        enragedImg.pixelsPerUnitMultiplier = 1;

        mutedGroup = BundleLoader.bundle.LoadAsset<AudioMixerGroup>("Mute");

        fadeOutAnim = fadeImg.gameObject.AddComponent<FadeOutAnim>();
    }

    public void Update()
    {
        if (NewMovement.instance != null)
        {
            if (!originalSpeedCaptured)
            {
                originalWalkSpeed = NewMovement.instance.walkSpeed;
                originalSpeedCaptured = true;
            }


            if (isEnraged)
            {
                NewMovement.instance.walkSpeed =
                    originalWalkSpeed * Plugin.speedMultiplier.Value;

            }
            else
            {
                NewMovement.instance.walkSpeed = originalWalkSpeed;
            }
        }
        if (isEnraged && SceneHelper.CurrentScene == "Main Menu")
            ToggleEnrage();

        if (rageEffect != null)
        {
            if (ClashPlayerEnabled() && isEnraged)
            {
                rageEffect.transform.position = PlatformerMovement.Instance.rb.transform.Find("v1_combined/metarig/spine/spine.001/spine.002/spine.003").position;
                rageEffect.transform.LookAt(Camera.main.transform);
            }
            else
            {
                Destroy(rageEffect);
                Enrage();
            }
        }
        else
            if (ClashPlayerEnabled() && isEnraged)
                Enrage();

        if (isEnraged)
        {
            if (Plugin.shakeScreen.Value)
                CameraController.Instance.CameraShake(.05f);
            if (Plugin.shakeWeapons.Value && CameraController.Instance != null && CameraController.Instance.hudCamera != null)
                CameraController.Instance.hudCamera.transform.Translate(UnityEngine.Random.Range(-1f, 1f) * Time.deltaTime, UnityEngine.Random.Range(-1f, 1f) * Time.deltaTime, 0);

            if ((!NewMovement.instance.activated || !NewMovement.instance.isActiveAndEnabled) && (PlatformerMovement.instance == null || (PlatformerMovement.instance != null && !PlatformerMovement.instance.activated)))
                source.volume -= (source.volume - 0) / (10f / (Time.unscaledDeltaTime * 60));
            else
                source.volume -= (source.volume - Plugin.enrageVolume.Value / 100f) / (10f / (Time.unscaledDeltaTime * 60));

            var vol = AudioMixerController.Instance.sfxVolume;
            if (Plugin.fadeSFX.Value)
                AudioMixerController.Instance.SetSFXVolume(vol - (vol - MathF.Min(vol, (Plugin.minimumSFXVolume.Value / 100f))) / (100f / (Time.unscaledDeltaTime * 60)));
        }

        if (Plugin.customMusicClip != null && (isEnraged || Plugin.alwaysPlayCustomMusic.Value) && SceneHelper.CurrentScene != "Main Menu" &&
            NewMovement.instance != null && (NewMovement.instance.gameObject.activeInHierarchy || (PlatformerMovement.instance != null && PlatformerMovement.instance.activated && PlatformerMovement.instance.gameObject.activeInHierarchy)))
        {
            music.volume = 1;
            if (!music.isPlaying)
            {
                music.outputAudioMixerGroup = AudioMixerController.Instance.musicGroup;
                music.clip = Plugin.customMusicClip;
                music.loop = true;
                music.Play();
            }
        }
        else
            music?.Stop();
    }

    public void ToggleEnrage()
    {
        isEnraged = !isEnraged;

        source.outputAudioMixerGroup = AudioMixerController.Instance.allGroup;

        Invoke(isEnraged ? "Enrage" : "CalmDown", 0);
    }

    public void Enrage()
    {
        enragedImg.color = new Color(1, 0, 0, 1);
        fadeImg.color = new Color(1, 0, 0, Plugin.screenFlashOpacity.Value / 100f);
        fadeOutAnim.targetAlpha = 0.1f;

        source.Stop();

        if (ClashPlayerEnabled())
        {
            if (rageEffect == null)
            {
                rageEffect = Instantiate(Plugin.Ass<GameObject>("Assets/Particles/Enemies/RageEffect.prefab"));
                rageEffect.GetComponentInChildren<AlwaysLookAtCamera>().preferCameraOverHead = true;
                DontDestroyOnLoad(rageEffect);
            }
        }
        else
        {
            source.PlayOneShot(Plugin.Ass<AudioClip>("Assets/Sounds/Enemies/enrage.wav"));
            source.clip = Plugin.Ass<AudioClip>("Assets/Sounds/Enemies/rageloop.wav");
            source.loop = true;
            source.Play();
        }
    }

    public void CalmDown()
    {
        enragedImg.color = new Color(1, 0, 0, 0);
        fadeImg.color = new Color(1, 1, 1, Plugin.screenFlashOpacity.Value / 100f);
        fadeOutAnim.targetAlpha = 0;

        source.Stop();
        if (!Plugin.alwaysPlayCustomMusic.Value)
            music.Stop();

        source.clip = Plugin.Ass<AudioClip>("Assets/Sounds/Enemies/EnrageEndPlayer.wav");
        source.loop = false;
        source.Play();

        AudioMixerController.Instance.sfxVolume = PrefsManager.Instance.GetFloat("sfxVolume");
        AudioMixerController.Instance.SetSFXVolume(AudioMixerController.Instance.sfxVolume);
        AudioMixerController.Instance.optionsMusicVolume = PrefsManager.Instance.GetFloat("musicVolume");
        AudioMixerController.Instance.SetMusicVolume(AudioMixerController.Instance.optionsMusicVolume);

        if (rageEffect != null)
            Destroy(rageEffect);
    }

    public static bool MusicPlaying() => instance.music != null && instance.music.isPlaying;

    bool ClashPlayerEnabled() => PlatformerMovement.Instance != null && PlatformerMovement.Instance.activated && PlatformerMovement.Instance.gameObject.activeInHierarchy;
}