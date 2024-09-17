using UnityEngine;

public class TankBody : MonoBehaviour
{
    [HideInInspector] public float forwardDirection;
    [HideInInspector] public float rotationDirection;

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 30f;


    [SerializeField] private GameObject[] bullets;

    private Rigidbody rb;

    private Transform fireLocation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        fireLocation = transform.Find("FireLocation");
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    private void Rotation()
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, rotationSpeed, 0) * rotationDirection * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void Movement()
    {
        // Z axis multiplied by desired forward direction and movement speed, normalized to delta time.
        Vector3 moveDirection = transform.forward * forwardDirection * movementSpeed * Time.fixedDeltaTime;

        rb.MovePosition(transform.position + moveDirection);
    }
}
