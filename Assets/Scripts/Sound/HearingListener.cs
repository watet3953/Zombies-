using UnityEngine;
using UnityEngine.Events;

public class HearingListener : MonoBehaviour
{
    public float hearingDisappationRatio = 1.0f;

    private Transform topSoundPosition = null;
    private float curPriority = 0.0f;

    void OnEnable() => HearingManager.instance.RegisterAudioListener(this);

    void OnDisable() => HearingManager.instance.DeregisterAudioListener(this);

    public Transform GetTopSoundPosition() => HasSound() ? topSoundPosition : null;

    public bool HasSound() => curPriority > 0.0f;

    public void Update() => curPriority -= Time.deltaTime * hearingDisappationRatio;

    public void RecieveSound(Transform t, float volume) {
        if (volume < curPriority) return; // not important enough, ignored.

        topSoundPosition = t;
        curPriority = volume;
    }
}
