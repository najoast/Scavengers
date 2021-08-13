using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource efxSource;
    public AudioSource musicSource;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip) {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips) {
        efxSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        int randomIndex = Random.Range(0, clips.Length);
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
}