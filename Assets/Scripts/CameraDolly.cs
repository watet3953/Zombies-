using UnityEngine;

public class CameraDolly : MonoBehaviour
{
    /// <summary> A reference to the transform the camera follows. </summary>
    [SerializeField] protected Transform toFollow;

    /// <summary> The offset between the camera's position and the target, 
    /// set at start. </summary>
    protected Vector3 offset;

    protected void Start()
    {
        Debug.Assert(
                toFollow != null,
                "Transform not assigned for CameraDolly."
            );

        offset = transform.position - toFollow.position;
    }

    protected void Update()
    {
        transform.position = toFollow.position + offset;
    }
}
