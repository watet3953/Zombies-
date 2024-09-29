using UnityEngine;

public class TankBody : MonoBehaviour
{
    /// <summary> User input for moving forward/backward/left/right. </summary>
    [HideInInspector] public Vector2 inputAxis;

    /// <summary> The top speed the player should move in a given direction, 
    /// formatted as (FORWARD, BACKWARD, LEFT, RIGHT). </summary>
    [SerializeField] private Vector4 movementSpeed = new(5f, 2f, 2f, 2f);


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

    /// <summary> The tank rigidbody, used for movement and aiming </summary>
    private Rigidbody rb;

    /// <summary> The transform of the barrel, used aiming. </summary>
    [SerializeField] private Transform barrelLocation;

    /// <summary> The transform at which the bullet is spawned. </summary>
    [SerializeField] private Transform fireLocation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    /// <summary> Handles the movement of the tank body. </summary>
    private void Movement()
    {
        // Z axis times desired direction and max speed, 
        // normalized to fixed delta time.
        Vector3 moveDirection = new(
                inputAxis.x > 0 
                        ? inputAxis.x * movementSpeed.z 
                        : inputAxis.x * movementSpeed.w,
                0,
                inputAxis.y > 0 
                        ? inputAxis.y * movementSpeed.x 
                        : inputAxis.y * movementSpeed.y
            );
        

        moveDirection *= Time.fixedDeltaTime;
        moveDirection = transform.rotation * moveDirection; // rotate to player

        rb.MovePosition(transform.position + moveDirection);
    }

    /// <summary>  Sets the direction the turret is currently aiming. </summary>
    /// <param name="aimDir"> The new rotation of the turret, normalized</param>
    public void UpdateAimDir(Vector3 aimDir)
    {
        aimDir.Normalize();

        rb.transform.rotation = Quaternion.LookRotation(aimDir);
    }

    /// <summary> Fires the specified bullet type from fireLocation. </summary>
    /// <param name="bulletType"> The type of bullet to fire.</param>
    public void Fire(BulletTypes bulletType)
    {
        Vector3 direction = fireLocation.position - barrelLocation.position;
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
