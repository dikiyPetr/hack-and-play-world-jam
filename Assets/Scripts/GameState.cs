using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public LevelSetup levelSetup;
    private int collected = 0;
    public RestartUI restartUI;

    private void Start()
    {
        collected = 0;
    }

    void Collect()
    {
        collected++;
        if (levelSetup.collectableCount == collected)
        {
            FinishLevel();
        }
    }

    void FinishLevel()
    {
    }

    public void OnDeathByMerge()
    {
        restartUI.Show();
    }
}