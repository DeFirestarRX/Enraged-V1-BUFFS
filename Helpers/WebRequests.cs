namespace Mod.Helpers;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequests
{
    static int timeout = 15;

    public static IEnumerator GetString(string url, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.timeout = timeout;
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Plugin.LogError("Failed to load string: " + www.error);
                callback?.Invoke(null);
            }
            else
            {
                callback?.Invoke(www.downloadHandler.text);
            }
        }
    }

    public static IEnumerator GetAudio(string url, System.Action<AudioClip> callback)
    {
        AudioType audType = url[(url.LastIndexOf('.') + 1)..].ToLower() switch
        {
            "wav" => AudioType.WAV,
            "ogg" => AudioType.OGGVORBIS,
            "mp3" => AudioType.MPEG,
            "mp4" => AudioType.MPEG,
            _ => AudioType.MPEG
        };

        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audType);
        www.timeout = 5;
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Plugin.LogError("Audio download error: " + www.error);
            callback(null);
        }
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            clip.name = Path.GetFileNameWithoutExtension(url);
            callback(clip);
        }
    }

    public static IEnumerator PostRequest(string url, Dictionary<string, string> postData, System.Action<string> callback)
    {
        WWWForm form = new WWWForm();
        foreach (var pair in postData)
        {
            form.AddField(pair.Key, pair.Value);
        }
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.timeout = timeout;
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Plugin.LogError("Failed to post request: " + www.error);
                callback?.Invoke(null);
            }
            else
            {
                callback?.Invoke(www.downloadHandler.text);
            }
        }
    }
}