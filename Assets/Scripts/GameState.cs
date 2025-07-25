using System;
using UnityEngine;

public enum StopGameType
{
    None,
    Finish,
    Death,
    Merge,
    Recursion,
}
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

    public void StopGame(StopGameType type)
    {
        restartUI.Show(type);
    }
}