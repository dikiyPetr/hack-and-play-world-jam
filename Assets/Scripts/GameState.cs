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
    private int collected = 0;
    public StopGameUI stopGameUI;
    public InputRecognizer inputRecognizer;
    private void Start()
    {
        collected = 0;
    }

    public void Collect()
    {
        collected++;
        if (levelSetup.collectableCount == collected)
        {
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
        inputRecognizer.DisableKeyboardListener();
        stopGameUI.Show(type);
    }
}