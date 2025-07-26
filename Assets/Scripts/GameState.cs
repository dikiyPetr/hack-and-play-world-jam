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

public class GameState : MonoBehaviour
{
    public LevelSetup levelSetup;
    private int collected = 0;
    public StopGameUI stopGameUI;
    public InputRecognizer inputRecognizer;
    public GameManager gameManager;
    private static int previousSceneId;

    private void OnEnable()
    {
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