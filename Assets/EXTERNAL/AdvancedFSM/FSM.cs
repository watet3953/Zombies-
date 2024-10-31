using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour
{
    //Player Transform
    protected Transform playerTransform;

    //Next destination position
    protected Vector3 destPos;

    //List of points for patrolling
    protected GameObject[] pointList;

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FSMFixedUpdate();
    }
}
