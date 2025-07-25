using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RestartUI : MonoBehaviour
{
    private VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.visible = false;
        Button playButton = root.Q<Button>("Button");
        if (playButton != null)
        {
            playButton.clicked += OnRestartClicked;
        }
    }

    public void Show()
    {
        root.visible = true;
    }

    private void OnRestartClicked()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}