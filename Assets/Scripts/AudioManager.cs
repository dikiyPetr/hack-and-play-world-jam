using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource step;
    [SerializeField] private AudioSource teleport;
    [SerializeField] private AudioSource ambient;
    [SerializeField] private AudioSource cat;
    [SerializeField] private AudioSource merge;
    [SerializeField] private AudioSource rats;
    [SerializeField] private AudioSource water;
    [SerializeField] private AudioSource lava;

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
        teleport.time = 0.7f;
        teleport.Play();
    }

    public void PlayRats()
    {
        rats.Play();
    }

    public void PlayWater()
    {
        water.Play();
    }

    public void PlayLava()
    {
        lava.Play();
    }

    public void PlayMerge()
    {
        merge.time = 0.5f;
        merge.Play();
    }

    public void PlayCat()
    {
        cat.Play();
    }
}