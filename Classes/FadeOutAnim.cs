namespace EnragedV1.Classes;

using UnityEngine;
using UnityEngine.UI;

public class FadeOutAnim : MonoBehaviour
{
    Image img;

    public float targetAlpha = 0;

    public void Start() => img = GetComponent<Image>();

    public void Update() => img.color = Color.Lerp(img.color, new Color(img.color.r, img.color.g, img.color.b, targetAlpha), Time.unscaledDeltaTime * 5);
}