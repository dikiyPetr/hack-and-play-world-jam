using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource step;
    [SerializeField] private AudioSource teleport;
    [SerializeField] private AudioSource ambient;

    void Start()
    {
        ambient.Play();
    }

    public void PlaySteps()
    {
        if (!step.isPlaying)
        {
            step.Play();
        }
    }

    public void StopSteps()
    {
        step.Stop();
    }

    public void PlayTeleport()
    {
        teleport.Play();
    }
}