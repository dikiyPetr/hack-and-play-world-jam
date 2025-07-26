using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HudUI : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Button playButton = root.Q<Button>("Restart");
        if (playButton != null)
        {
            playButton.clicked += OnRestartClicked;
        }
    }

    private void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}