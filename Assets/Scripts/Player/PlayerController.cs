using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //TODO: add camera panning in direction of aiming for scoped in stuff.
    /// <summary> The camera attached to the tank. Not used yet </summary>
    [SerializeField] private Camera activeCamera;

    /// <summary> The interaction manager. </summary>
    [SerializeField] private InteractableManager interactionManager;

    /// <summary> The physical tank attached to the input. </summary>
    private PlayerBody body;

    /// <summary> Enables Debug output. </summary>
    [SerializeField] private bool debug = false;

    // GetComponentInChildren<PlayerBody>();
    private void Start() => body = transform.Find("PlayerBody").GetComponent<PlayerBody>(); 

    /// <summary> Rotates the player towards the provided aim. </summary>
    /// <param name="inputValue"> The Input as a Vector2. </param>
    public void OnLook(InputValue inputValue)
    {
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
        body.Fire(PlayerBody.BulletTypes.Default);
    }

    /// <summary> The alternate fire input, fires a expanding bullet. </summary>
    /// <param name="inputValue"> The shot input, discarded. </param>
    public void OnAltFire(InputValue inputValue)
    {
        body.Fire(PlayerBody.BulletTypes.Expanding);
    }

    public void OnInteract(InputValue inputValue)
    {
        interactionManager.Interact();
    }

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
