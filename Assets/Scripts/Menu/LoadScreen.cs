using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] Slider loadBar;

    void UpdateSlider(float value)
    {
        value = Mathf.Clamp01(value);

        loadBar.value = value;
    }

    public IEnumerator FakeLoad(float duration = 1f)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            UpdateSlider(timer/duration);
            yield return null;
        }

        // clamp to 1 manually
        UpdateSlider(1f);
    }
}
