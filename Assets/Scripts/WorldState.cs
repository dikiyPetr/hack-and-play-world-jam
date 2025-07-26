using UnityEngine;

public class WorldState : MonoBehaviour
{
    public static WorldState Instance { get; private set; }

    private void Awake()
    {
        Debug.Log(1);
        if (Instance != null && Instance != this)
        {
            Debug.Log(2);
            Destroy(gameObject); // Удалить дубликат
            return;
        }

        Debug.Log(3);
        Instance = this;
        DontDestroyOnLoad(gameObject); // Сохраняем при смене сцены
        Debug.Log(4);
    }

    private int _attempts = 0;
    private int _timeMs = 0;
    public int attemptsInLevel;

    public int attempts => _attempts;
    public int timeMs => _timeMs;

    public string timeFormat => Utils.FormatTime(_timeMs);


    public void AddAttempt()
    {
        _attempts++;
    }

    public void AddTime(int timeMs)
    {
        _timeMs += timeMs;
    }
}