using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NextLevelUI : MonoBehaviour
{
    private VisualElement root;
    
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.visible = false;
        Button playButton = root.Q<Button>("Button");
        if (playButton != null)
        {
            playButton.clicked += OnNextLevel;
        }
    }

    private void OnNextLevel()
    {
        //todo next level
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Show()
    {
        root.visible = true;
    }
}