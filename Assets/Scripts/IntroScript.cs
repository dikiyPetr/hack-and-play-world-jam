using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextFadeOut : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float delayBeforeFade = 1f;
    public float fadeDuration = 2f;
    public SceneName nextScene;

    private float timer = 0f;
    private bool fading = false;
    private bool opening = false;

    void Start()
    {
        // Устанавливаем начальную прозрачность на 1 (полностью видим)
        Color color = text.color;
        color.a = 1f;
        text.color = color;

        timer = delayBeforeFade;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        if (!fading)
        {
            fading = true;
        }

        if (fading)
        {
            Color color = text.color;
            color.a -= Time.deltaTime / fadeDuration;
            text.color = color;

            if (color.a <= 0f)
            {
                if (opening)
                {
                    return;
                }

                // Загружаем следующую сцену
                opening = true;
                SceneManager.LoadScene(nextScene.ToString());
            }
        }
    }
}