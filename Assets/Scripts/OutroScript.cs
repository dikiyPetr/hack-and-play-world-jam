using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextFadeSequence : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string firstMessage = "The mistake has been undone.\nNine feline souls, offered in sacrifice, have become the new seal in your place.\nYou are now free… but they are bound to each other forever";
    public string secondMessage = "Thank you for playing";
    public float delayBeforeFadeIn = 1f;
    public float fadeDuration = 2f;
    public float delayBetweenMessages = 10f;

    private float timer = 0f;
    private int phase = 0; // 0: wait, 1: fade in first, 2: fade out first, 3: fade in second, 4: fade out second, 5: load scene
    private Color textColor;

    void Start()
    {
        text.text = firstMessage;
        textColor = text.color;
        textColor.a = 0f;
        text.color = textColor;
        timer = delayBeforeFadeIn;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        switch (phase)
        {
            case 0: // Delay before fade-in first
                if (timer <= 0f)
                {
                    phase = 1;
                    timer = fadeDuration;
                }
                break;

            case 1: // Fade in first message
                textColor.a = 1f - Mathf.Clamp01(timer / fadeDuration);
                text.color = textColor;
                if (timer <= 0f)
                {
                    phase = 2;
                    timer = fadeDuration;
                }
                break;

            case 2: // Fade out first message
                textColor.a = Mathf.Clamp01(timer / fadeDuration);
                text.color = textColor;
                if (timer <= 0f)
                {
                    text.text = secondMessage;
                    phase = 3;
                    timer = fadeDuration;
                }
                break;

            case 3: // Fade in second message
                textColor.a = 1f - Mathf.Clamp01(timer / fadeDuration);
                text.color = textColor;
                if (timer <= 0f)
                {
                    phase = 4;
                    timer = fadeDuration + delayBetweenMessages;
                }
                break;
        }
    }
}
