using UnityEngine;
using UnityEngine.InputSystem;

public class TankPlayerController : MonoBehaviour
{
    //TODO: add camera panning in direction of aiming for scoped in stuff.
    /// <summary> The camera attached to the tank. Not used yet </summary>
    [SerializeField] private Camera activeCamera;

    /// <summary>
    /// The range to interact with.
    /// </summary>
    [SerializeField] private InteractableManager interactionManager;

    /// <summary> The physical tank attached to the input. </summary>
    private TankBody body;

    /// <summary> Enables Debug output. </summary>
    [SerializeField] private bool debug = false;

    private void Start()
    {
        body = GetComponentInChildren<TankBody>();
    }

    public void OnLook(InputValue inputValue)
    {
        UpdateAim(inputValue.Get<Vector2>());
    }

    /// <summary> Converts movement input into axis to the tankbody. </summary>
    public void OnMove(InputValue inputValue)
    {
        Vector2 direction = inputValue.Get<Vector2>();

        body.forwardInput = direction.y;
        body.rotationInput = direction.x;
    }

    public void OnFire(InputValue inputValue)
    {
        body.Fire(TankBody.BulletTypes.Default);
    }

    public void OnInteract(InputValue inputValue)
    {
        interactionManager.Interact();
    }

    private void UpdateAim(Vector2 aimPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(aimPos);

        Plane plane = new(body.turret.up, body.turret.position);


        if (plane.Raycast(ray, out float distanceToPlane))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distanceToPlane);

            Vector3 aimDirection = mouseWorldPos - body.turret.position;
            aimDirection.y = 0f;

            body.UpdateAimDir(aimDirection);

            if (!debug) return;

            Debug.DrawLine(body.transform.position, mouseWorldPos, Color.green);
            Debug.DrawLine(ray.origin, mouseWorldPos, Color.magenta);

            Debug.DrawRay(mouseWorldPos, Vector3.left, Color.cyan);
        }
    }
}
