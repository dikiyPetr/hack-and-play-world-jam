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

    public int attempts => _attempts;
    public int timeMs => _timeMs;

    public string timeFormat => FormatTime(_timeMs);


    public void AddAttempt()
    {
        _attempts++;
    }

    public void AddTime(int timeMs)
    {
        _timeMs += timeMs;
    }

    private string FormatTime(float milliseconds)
    {
        int totalMs = Mathf.FloorToInt(milliseconds);
        int minutes = totalMs / 60000;
        int seconds = (totalMs % 60000) / 1000;
        int ms = totalMs % 1000;

        return $"{minutes:00}:{seconds:00}:{ms:00}";
    }
}