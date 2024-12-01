using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //TODO: add camera panning in direction of aiming for scoped in stuff.
    /// <summary> The camera attached to the tank. Not used yet. </summary>
    [SerializeField] protected Camera activeCamera;

    /// <summary> The interaction manager. </summary>
    [SerializeField] protected InteractableManager interactionManager;

    /// <summary> The physical tank attached to the input. </summary>
    [SerializeField] protected PlayerBody body;
    [HideInInspector] public PlayerBody Body => body;

    /// <summary> Enables Debug output. </summary>
    [SerializeField] protected bool debug = false;

    protected void Start()
    {
        Debug.Assert(body != null);
    }

    /// <summary> Rotates the player towards the provided aim. </summary>
    /// <param name="inputValue"> The Input as a Vector2. </param>
    public void OnLook(InputValue inputValue)
    {
        if (body.enabled)
            UpdateAim(inputValue.Get<Vector2>());
    }

    /// <summary> Moves the player in the specified direction. </summary>
    public void OnMove(InputValue inputValue)
    {
        body.inputAxis = inputValue.Get<Vector2>();
    }

    /// <summary> The primary fire input, shoots. </summary>
    /// <param name="inputValue"> The shot input, discarded. </param>
    public void OnFire(InputValue inputValue)
    {
        if (body.enabled) 
            body.Fire(PlayerBody.BulletTypes.Default);
    }

    /// <summary> The alternate fire input, fires a expanding bullet. </summary>
    /// <param name="inputValue"> The shot input, discarded. </param>
    public void OnAltFire(InputValue inputValue)
    {
        if (body.enabled)
            body.Fire(PlayerBody.BulletTypes.Expanding);
    }

    /// <summary> The interaction input, triggers an interaction. </summary>
    /// <param name="inputValue"> The interaction input, discarded. </param>
    public void OnInteract(InputValue inputValue)
    {
        if (body.enabled)
            interactionManager.Interact();
    }

    /// <summary> Updates the aim of the player according to the position 
    /// of the mouse on the screen. </summary>
    /// <param name="aimPos"> The position of the mouse on the screen. </param>
    private void UpdateAim(Vector2 aimPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(aimPos);

        Plane plane = new(body.transform.up, body.transform.position);


        if (plane.Raycast(ray, out float distanceToPlane))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distanceToPlane);

            Vector3 aimDirection = mouseWorldPos - body.transform.position;
            aimDirection.y = 0f;

            body.UpdateAimDir(aimDirection);

            if (!debug) return;

            Debug.DrawLine(body.transform.position, mouseWorldPos, Color.green);
            Debug.DrawLine(ray.origin, mouseWorldPos, Color.magenta);

            Debug.DrawRay(mouseWorldPos, Vector3.left, Color.cyan);
        }
    }
}
