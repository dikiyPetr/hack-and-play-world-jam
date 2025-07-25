using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RestartUI : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
   
        Button playButton = root.Q<Button>("RestartButton");
        if (playButton != null)
        {
            playButton.clicked += OnPlayClicked;
        }
    }

    private void OnPlayClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}