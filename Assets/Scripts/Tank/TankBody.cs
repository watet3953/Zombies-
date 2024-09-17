using UnityEngine;

public class TankBody : MonoBehaviour
{
    [HideInInspector] public float forwardDirection;
    [HideInInspector] public float rotationDirection;

    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 30f;

    public enum BulletTypes { Default, }
    [SerializeField] private GameObject[] bullets;

    private Rigidbody rb;

    [SerializeField] private Transform fireLocation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    public void Fire(BulletTypes bulletType)
    {
        Vector3 direction = fireLocation.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        Bullet bullet = Instantiate(bullets[(int)(bulletType)], fireLocation.position, Quaternion.identity).GetComponent<Bullet>();

        bullet.gameObject.SetActive(true);
        bullet.Fire(direction);
    }
}
