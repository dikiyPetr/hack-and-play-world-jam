using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HudUI : MonoBehaviour
{
    private Label _cats;
    private Label _attemps;
    [SerializeField] private GameState gameState;
    private VisualElement _root;
    private Label _timer;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _cats = _root.Q<Label>("cats");
        _attemps = _root.Q<Label>("attemps");
        _timer = _root.Q<Label>("timer");
    }

    private void Update()
    {
        _cats.text = $"{gameState.collected} | {gameState.levelSetup.collectableCount.ToString()}";
        _attemps.text = WorldState.Instance.attempts.ToString();
        _timer.text = WorldState.Instance.timeFormat;
    }

    private void OnEnable()
    {
        Button playButton = _root.Q<Button>("Restart");
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