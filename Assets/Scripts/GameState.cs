using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StopGameType
{
    None,
    Finish,
    Restart,
    DeathByLava,
    DeathByWater,
    DeathByRats,
    Merge,
    Recursion,
}

public enum SceneName
{
    IntroLevel,
    TutorialLevel,
    TeleportTutorialLevel,
    TeleporAndMergeLevel,
    RatsLevel,
    OutroLevel,
}

public class GameState : MonoBehaviour
{
    public LevelSetup levelSetup;
    private int _collected = 0;
    public StopGameUI stopGameUI;
    public InputRecognizer inputRecognizer;
    private bool isStaredTimer = false;
    public int collected => _collected;
    private int _timeMs = 0;
    public int timeMs => _timeMs;
    public GameManager gameManager;
    private static int previousSceneId = -1;

    public string timeFormat => Utils.FormatTime(_timeMs);

    private void Start()
    {
        _collected = 0;
        isStaredTimer = true;

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

    private void Update()
    {
        if (isStaredTimer)
        {
            var ms = (int)(Time.deltaTime * 1000);
            WorldState.Instance.AddTime(ms);
            AddTime(ms);
        }
    }

    private void AddTime(int timeMs)
    {
        _timeMs += timeMs;
    }

    public void Collect()
    {
        gameManager.audioManager.PlayCat();
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
        if (type == StopGameType.Restart)
        {
            WorldState.Instance.attemptsInLevel++;
            WorldState.Instance.AddAttempt();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        isStaredTimer = false;
        inputRecognizer.DisableKeyboardListener();
        stopGameUI.Show(type);
    }

    public void NextLvl()
    {
        SceneManager.LoadScene(levelSetup.nextScene.ToString());
        WorldState.Instance.attemptsInLevel = 0;
    }
}