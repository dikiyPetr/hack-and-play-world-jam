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

    public void Show(StopGameType type)
    {
        root.visible = true;

        if (type == StopGameType.Recursion)
        {
            root.Q<Label>("Label").text = "Закатался на карусели";
            return;
        }

        if (type == StopGameType.Merge)
        {
            root.Q<Label>("Label").text = "СЛИЯНИЕЕЕЕ!";
            return;
            
        }  
        if (type == StopGameType.Death)
        {
            root.Q<Label>("Label").text = "ЭТО СМЕРТЬ!";
            return;
            
        }

        root.Q<Label>("Label").text = "ЛОХ, ИДИ ИГРУ ДЕЛАЙ!";
    }

    private void OnRestartClicked()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}