using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingEmitter : MonoBehaviour
{

    public AudioSource audioSource;
    float volume;

    public void Play() {
        audioSource.Play();
        HearingManager.instance.EmitSound(transform, volume);
    }
}
