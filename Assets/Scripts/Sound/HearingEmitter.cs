using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingEmitter : MonoBehaviour
{

    public AudioSource audioSource;
    public float volume;

    public void Play() {
        audioSource.Play();
        HearingManager.instance.EmitSound(transform.position, volume);
    }

    public void PlayOneShot()
    {
        audioSource.PlayOneShot(audioSource.clip);
        HearingManager.instance.EmitSound(transform.position, volume);
    }
}
