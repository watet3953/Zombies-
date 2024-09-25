using UnityEngine;

public class TankPlayerController : MonoBehaviour
{
    /// <summary> The physical tank attached to the input. </summary>
    private TankBody body;

    /// <summary> Enables Debug output. </summary>
    [SerializeField] private bool debug = false;

    private void Start()
    {
        body = GetComponentInChildren<TankBody>();
    }


    private void Update()
    {
        MovementHandler();
        UpdateAim();

        // Left click fires normal bullet, right click fires expanding.
        if (Input.GetButtonDown("Fire1"))
        { 
            body.Fire(TankBody.BulletTypes.Default); 
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            body.Fire(TankBody.BulletTypes.Expanding);
        }

    }

    /// <summary> Converts movement input into axis to the tankbody. </summary>
    private void MovementHandler()
    {
        Vector2 direction = new(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );

        body.forwardInput = direction.y;
        body.rotationInput = direction.x;
    }

    private void UpdateAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(body.turret.up, body.turret.position);

        float distanceToPlane;

        if (plane.Raycast(ray, out distanceToPlane))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distanceToPlane);

            Vector3 aimDirection = mouseWorldPos - body.turret.position;
            aimDirection.y = 0f;

            body.UpdateAimDir(aimDirection);

            if (debug)
            {
                Debug.DrawLine(body.transform.position, mouseWorldPos, Color.green);
                Debug.DrawLine(ray.origin, mouseWorldPos, Color.magenta);

                Debug.DrawRay(mouseWorldPos, Vector3.left, Color.cyan);
            }
        }
    }
}
