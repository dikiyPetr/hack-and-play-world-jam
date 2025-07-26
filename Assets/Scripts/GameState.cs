using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StopGameType
{
    None,
    Finish,
    Death,
    Merge,
    Recursion,
}

public enum SceneName
{
    TutorialLevel,
    TeleportTutorialLevel,
    TeleporAndMergeLevel,
    RatsLevel,
}

public class GameState : MonoBehaviour
{
    public LevelSetup levelSetup;
    private int _collected = 0;
    public StopGameUI stopGameUI;
    public InputRecognizer inputRecognizer;
    private bool isStaredTimer = false;
    public int collected => _collected;

    private void Start()
    {
        _collected = 0;
        isStaredTimer = true;
    }

    private void Update()
    {
        if (isStaredTimer)
        {
            WorldState.Instance.AddTime((int) (Time.deltaTime* 1000));
        }
    }

    public void Collect()
    {
        _collected++;
        if (levelSetup.collectableCount == _collected)
        {
            isStaredTimer = false;
            FinishLevel();
        }
    }

    void FinishLevel()
    {
        stopGameUI.ActivateNextLvl();
        stopGameUI.Show(StopGameType.Finish);
    }

    public void StopGame(StopGameType type)
    {
        isStaredTimer = false;
        WorldState.Instance.AddAttempt();
        inputRecognizer.DisableKeyboardListener();
        stopGameUI.Show(type);
    }
}