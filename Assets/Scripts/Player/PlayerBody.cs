﻿using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    /// <summary> User input for moving forward/backward/left/right. </summary>
    [HideInInspector] public Vector2 inputAxis;

    /// <summary> The top speed the player should move in a given direction, 
    /// formatted as (FORWARD, BACKWARD). </summary>
    [SerializeField] protected Vector2 movementSpeed = new(5f, 2f);


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
    [SerializeField] protected GameObject[] bullets;

    /// <summary> The tank rigidbody, used for movement and aiming </summary>
    protected Rigidbody rb;

    /// <summary> The transform of the barrel, used aiming. </summary>
    [SerializeField] protected Transform barrelLocation;

    /// <summary> The transform at which the bullet is spawned. </summary>
    [SerializeField] protected Transform fireLocation;

    protected void Awake()
    {
        Debug.Assert(
                GetComponent<Rigidbody>() != null,
                "Could not find rigidbody for Player."
            );
        rb = GetComponent<Rigidbody>();
    }

    protected void FixedUpdate()
    {
        Movement();
    }

    /// <summary> Handles the movement of the player body. </summary>
    protected void Movement()
    {

        Vector3 moveDirection = new(inputAxis.x, 0, inputAxis.y);
        moveDirection.Normalize();

        // set movement speed based on difference between forward and movement.
        moveDirection *= Mathf.Lerp(
                movementSpeed.x,
                movementSpeed.y,
                Vector3.Angle(moveDirection, transform.forward) / 180f
            );

        moveDirection *= Time.fixedDeltaTime;

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
