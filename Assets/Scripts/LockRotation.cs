using UnityEngine;

public class LockRotation : MonoBehaviour
{
    /// <summary> Lock the X axis of rotation. </summary>
    [SerializeField] protected bool lockX;
    
    /// <summary> Lock the Y axis of rotation. </summary>
    [SerializeField] protected bool lockY;
    
    /// <summary> Lock the Z axis of rotation. </summary>
    [SerializeField] protected bool lockZ;

    /// <summary> The initial rotation of the object. </summary>
    protected Quaternion initRotation;

    private void Awake()
    {
        initRotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(
                lockX ? initRotation.eulerAngles.x : transform.eulerAngles.x,
                lockY ? initRotation.eulerAngles.y : transform.eulerAngles.y,
                lockZ ? initRotation.eulerAngles.z : transform.eulerAngles.z
            );
    }
}
