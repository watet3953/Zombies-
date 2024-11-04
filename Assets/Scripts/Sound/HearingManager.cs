using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingManager : MonoBehaviour
{
    public static HearingManager instance = null;

    public float soundDissapationRatio = 1.0f;

    public List<HearingListener> listeners = new();

    void Awake() {
        if (instance != null) { // lazy singleton.
            Debug.LogError("Multiple HearingManagers Detected, destroying old manager.");
            Destroy(instance);
            return;
        }
        instance = this;
    }

    public void EmitSound(Transform position, float volume) {
        foreach (HearingListener listener in listeners) { // this could be made more efficient with like a quad tree or something if scaled up a shit ton.
            float distance = (listener.transform.position - position.position).magnitude;
            if (distance * soundDissapationRatio < volume) {
                listener.RecieveSound(position, volume - distance);
            }
        }
    }

    public void RegisterAudioListener(HearingListener listener) {
        listeners.Add(listener);
    }

    public void DeregisterAudioListener(HearingListener listener) {
        listeners.Remove(listener);
    }
}
