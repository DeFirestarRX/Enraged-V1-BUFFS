namespace Mod;

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Mod.Classes;
using Mod.Helpers;
using Mod.Helpers.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin instance;
    public static ConfigFile config;
    public static ConfigEntry<float> speedMultiplier;
    public static ConfigEntry<float> healthMultiplier;
    public static ConfigEntry<float> damageMultiplier;
    public static ConfigEntry<int> screenFlashOpacity;
    public static ConfigEntry<int> enrageVolume;
    public static ConfigEntry<int> minimumSFXVolume;

    public static ConfigEntry<string> customMusic;
    public static ConfigEntry<bool> alwaysPlayCustomMusic;

    public static ConfigEntry<bool> fadeSFX;
    public static ConfigEntry<bool> shakeScreen;
    public static ConfigEntry<bool> shakeWeapons;

    public static AudioClip customMusicClip;

    public void Awake()
    {
        instance = this;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        new Harmony(PluginInfo.GUID).PatchAll();

        config = Config;

        screenFlashOpacity = config.Bind("Visuals", "Screen flash opacity", 50, "(0 - 100) Sets the opacity the flash does every time you toggle enrage mode");
        shakeScreen = config.Bind("Visuals", "Shake screen", false, "Shakes the screen while you're enraged");
        shakeWeapons = config.Bind("Visuals", "Shake weapons", true, "Shakes your weapons while you're enraged");

        enrageVolume = config.Bind("Audio", "Enrage volume", 50, "(0 - 100) Sets the volume for enrage audio (doesn't work in clash mode)");
        customMusic = config.Bind("Audio", "Custom music path", "", "Set an url or file path to play custom music instead of the normal one when enraged (leave empty for no music change, file paths start with \" file:/// \" for some reason) (requires game restart)");
        alwaysPlayCustomMusic = config.Bind("Audio", "Always play custom music", false, "Makes custom music always play even if you're not enraged");
        fadeSFX = config.Bind("Audio", "Gradually fade SFX", false, "Gradually fades out all in-game SFX while enraged");
        minimumSFXVolume = config.Bind("Audio", "Minimum SFX volume", 10, "The minimum volume the SFX will have if Gradually fade SFX is enabled");

        speedMultiplier = config.Bind("Buffs", "Speed Multiplier", 1.5f, "Movement speed multiplier while enraged (1.0 = normal speed)");
        damageMultiplier = config.Bind("Buffs", "Damage Multiplier", 2.0f, "Damage multiplier while enraged");
        LogInfo("Commissioned by PrismaticYaya <3");
        LogInfo("Buffs Added by @the_pure_vanilla_cookie (Id: 418051775329992704) on discord!");
    }

    public void Start()
    {
        BundleLoader.LoadBundle("Assets.enragedv1.bundle");

        if (customMusic.Value != "") StartCoroutine(WebRequests.GetAudio(customMusic.Value, (clip) => customMusicClip = clip ));

        foreach (var (type, attr) in AttributeHelper.GetTypesWithAttribute<CreateOnStart>())
        {
            if (type == null) continue;
            if (typeof(MonoBehaviour).IsAssignableFrom(type))
            {
                GameObject obj = new GameObject(type.Name);
                DontDestroyOnLoad(obj);
                obj.AddComponent(type);
            }
        }
    }

    /// Amazing code made by Bryan_-000-
    /// <summary> Cache list of all used addressable assets. </summary>
    public static Dictionary<string, object> CachedAddressableAssets = [];

    /// <summary> Synchronously loads an asset via addressables with its addressable key. </summary>
    public static T Ass<T>(string key)
    {
        if (CachedAddressableAssets.TryGetValue(key + typeof(T).Name, out object cachedAsset))
            return (T)cachedAsset;

        T asset = Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
        CachedAddressableAssets.Add(key + typeof(T).Name, asset);

        return asset;
    }

    public static void LogInfo(object msg) => instance.Logger.LogInfo(msg);
    public static void LogWarning(object msg) => instance.Logger.LogWarning(msg);
    public static void LogError(object msg) => instance.Logger.LogError(msg);
}

public class PluginInfo
{
    public const string GUID = "duviz.EnragedV1";
    public const string Name = "EnragedV1";
    public const string Version = "0.1.0";
}