using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;
    private Quaternion initRotation;

    private void Awake() => initRotation = transform.rotation;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(
                lockX ? initRotation.eulerAngles.x : transform.eulerAngles.x,
                lockY ? initRotation.eulerAngles.y : transform.eulerAngles.y,
                lockZ ? initRotation.eulerAngles.z : transform.eulerAngles.z);
    }
}
