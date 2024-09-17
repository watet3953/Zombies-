using UnityEngine;

public class TankPlayerController : MonoBehaviour
{
    /// <summary> The physical body set to recieve input from the player </summary>
    private TankBody body;

    private void Start()
    {
        body = GetComponentInChildren<TankBody>();
    }


    private void Update()
    {
        MovementHandler();

        if (Input.GetButtonDown("Fire1")) body.Fire(TankBody.BulletTypes.Default);

    }

    private void MovementHandler()
    {
        Vector2 direction = new(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );

        body.forwardDirection = direction.y;
        body.rotationDirection = direction.x;
    }
}
