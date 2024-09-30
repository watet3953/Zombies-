using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDolly : MonoBehaviour
{

    [SerializeField] private Transform toFollow;

    private Vector3 offset;

    void Start() => offset = transform.position - toFollow.position;

    void Update()
    {
        transform.position = toFollow.position + offset;
    }
}
