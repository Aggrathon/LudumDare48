using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{

    public AudioClip[] errorSounds;
    public AudioClip[] moveSounds;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayError()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(errorSounds[Random.Range(0, errorSounds.Length)]);
    }

    public void PlaySteps()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(moveSounds[Random.Range(0, moveSounds.Length)]);
    }
}
