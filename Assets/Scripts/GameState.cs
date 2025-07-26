using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StopGameType
{
    None,
    Finish,
    DeathByLava,
    DeathByWater,
    DeathByRats,
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
    public GameManager gameManager;
    private static int previousSceneId;
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
        collected = 0;
        if (SceneManager.GetActiveScene().buildIndex == previousSceneId)
        {
            gameManager.demonController.HideSay();
            return;
        }

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                gameManager.demonController.Say(StringAssets.MessageLevel1);
                break;
            case 2:
                gameManager.demonController.Say(StringAssets.MessageLevel2);
                break;
            case 3:
                gameManager.demonController.Say(StringAssets.MessageLevel3);
                break;
            case 4:
                gameManager.demonController.Say(StringAssets.MessageLevel4);
                break;
        }

        previousSceneId = SceneManager.GetActiveScene().buildIndex;
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