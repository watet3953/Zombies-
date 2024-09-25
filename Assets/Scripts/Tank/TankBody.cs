using UnityEngine;

public class TankBody : MonoBehaviour
{
    /// <summary> Input from user for moving forward/backward. </summary>
    [HideInInspector] public float forwardInput;

    /// <summary> Input from user for turning left/right. </summary>
    [HideInInspector] public float rotationInput;

    /// <summary> The top speed the player should normally move. </summary>
    [SerializeField] private float movementSpeed = 5f;

    /// <summary> The top speed the player should normally rotate. </summary>
    [SerializeField] private float rotationSpeed = 30f;


    /// <summary> The types of bullet the tank can fire. </summary>
    public enum BulletTypes 
    { 
        /// <summary> Impacts, damages, and despawns. </summary>
        Default, 

        /// <summary> Impacts, expands while damaging, then fades. </summary>
        Expanding  
    }

    /// <summary> An array of bullet prefabs, with array index corresponding 
    /// to BulletTypes enum value. </summary>
    [SerializeField] private GameObject[] bullets;

    /// <summary> The tank rigidbody, used for movement. </summary>
    private Rigidbody rb;

    /// <summary> The turret of the tank, used for aiming. </summary>
    [SerializeField] public Transform turret;

    /// <summary> The transform at which the bullet is spawned. </summary>
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

    /// <summary> Handles the rotation of the tank body. </summary>
    private void Rotation()
    {
        // Get desired body rotation via user input around the y axis
        Quaternion deltaRotation = Quaternion.Euler(
                rotationInput 
                * Time.fixedDeltaTime 
                * new Vector3(0, rotationSpeed, 0));

        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    /// <summary> Handles the movement of the tank body. </summary>
    private void Movement()
    {
        // Z axis times desired direction and max speed, 
        // normalized to fixed delta time.
        Vector3 moveDirection = forwardInput 
                * movementSpeed 
                * Time.fixedDeltaTime 
                * transform.forward;

        rb.MovePosition(transform.position + moveDirection);
    }

    /// <summary>  Sets the direction the turret is currently aiming. </summary>
    /// <param name="aimDir"> The new rotation of the turret, normalized</param>
    public void UpdateAimDir(Vector3 aimDir)
    {
        aimDir.Normalize();

        turret.transform.rotation = Quaternion.LookRotation(aimDir);
    }

    /// <summary> Fires the specified bullet type from fireLocation. </summary>
    /// <param name="bulletType"> The type of bullet to fire.</param>
    public void Fire(BulletTypes bulletType)
    {
        Vector3 direction = fireLocation.position - transform.position;
        direction.y = (bulletType == BulletTypes.Expanding) ? 1.2f : 0f;
        direction.Normalize();

        Bullet bullet = Instantiate(
                bullets[(int)bulletType],
                fireLocation.position,
                Quaternion.identity
            ).GetComponent<Bullet>();

        bullet.gameObject.SetActive(true);
        bullet.Fire(direction);
    }
}
