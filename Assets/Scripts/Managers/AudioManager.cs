using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadLevelComplete()
    {
        if (MapManager.Instance)
        {
            AudioClip music = MapManager.Instance.music;

            if (music)
            {
                musicSource.clip = music;
                musicSource.loop = true;
                musicSource.spatialBlend = 0f;
                musicSource.Play();
            }
            FadeIn();
        }
    }

    private float masterVolume;
    private float musicVolume;
    private float SFXVolume;
    [SerializeField] private AudioMixer mixer;

    private void Start()
    {
        mixer.GetFloat("MasterVolume", out masterVolume);
        mixer.GetFloat("MusicVolume", out musicVolume);
        mixer.GetFloat("SFXVolume", out SFXVolume);
    }

    public void FadeIn()
    {
        StartCoroutine(LerpVolume(-80f, masterVolume, 1f));
    }

    public IEnumerator FadeOut()
    {
        yield return StartCoroutine(LerpVolume(masterVolume, -80f, 1f));
    }

    IEnumerator LerpVolume(float startVolume, float endVolume, float duration)
    {
        float curVolume = startVolume;
        float curTime = 0f;

        while (curTime < duration) // while not finished duration, keep lerping the master vol towards the endVolume.
        {
            curTime += Time.deltaTime;
            curTime = Mathf.Clamp(curTime, 0f, duration);
            curVolume = Mathf.Lerp(startVolume, endVolume, curTime / duration);

            mixer.SetFloat("MasterVolume", curVolume);

            yield return null;
        }

    }

}
