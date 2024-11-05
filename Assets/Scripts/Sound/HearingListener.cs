using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HearingListener : MonoBehaviour
{
    public float hearingDisappationRatio = 5.0f;

    private Vector3? topSoundPosition = null;
    [SerializeField] private float curPriority = 0.0f;

    private bool newSound = false;

    void OnEnable() => StartCoroutine(AwaitConnection());

    private IEnumerator AwaitConnection() {
        while (HearingManager.instance == null) { // this is a nasty spinlock but it always seems to have bad timing when called synchronously, and I can't really set up an event.
            yield return new WaitForSeconds(0.01f);
        }
        HearingManager.instance.RegisterAudioListener(this);
        Debug.Log("Connected Listener \"" + name + "\" to HearingManager");
    }

    void OnDisable() => HearingManager.instance.DeregisterAudioListener(this);

    public Vector3? GetTopSoundPosition() => HasSound() ? topSoundPosition : null;

    public bool HasSound() => curPriority > 0.0f;

    public bool HasNewSound() {
        if (newSound) {
            newSound = false;
            return true;
        }
        return false;
    }

    public void Update() => curPriority -= Time.deltaTime * hearingDisappationRatio;

    public void RecieveSound(Vector3 t, float volume) {
        if (volume < curPriority) return; // not important enough, ignored.

        topSoundPosition = t;
        curPriority = volume;
        newSound = true;
    }
}
