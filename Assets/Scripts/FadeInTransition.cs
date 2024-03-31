using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeInTransition : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    
    private Image image;
    private Color originalColor;
    private float originalTransparency = 0.5f;
    public bool fadeInStarted = false;
    private float fadeInStartTime;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        // originalTransparency = originalColor.a;

        Color transparentColor = originalColor;
        transparentColor.a = 0f;
        image.color = transparentColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeInStarted) {
            float elapsedTime = Time.time - fadeInStartTime;
            // Clamps the value between 0 and the original transparency
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration) * originalTransparency;

            Color currentColor = originalColor;
            currentColor.a = alpha;
            image.color = currentColor;

            if (alpha >= originalTransparency) {
                fadeInStarted = false;
            }
        }
    }

    public void TriggerFadeIn() {
        fadeInStarted = true;
        fadeInStartTime = Time.time;
    }
}
