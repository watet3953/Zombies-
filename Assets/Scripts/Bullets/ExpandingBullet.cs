using System.Collections;
using UnityEngine;

public class ExpandingBullet : Bullet
{
    /// <summary> How large the bullet expands to in units diameter. </summary>
    [SerializeField] protected float expandSize = 5f;

    /// <summary> The Rigidbody for the bullet. </summary>
    protected Rigidbody rb;

    [SerializeField] protected HearingEmitter emitter;

    private bool blownUp = false;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Assert(rb != null, "Could not get Rigidbody.");
    }

    protected new void Start() { } // To wipe functionality of superclass.

    protected new void Update() { } // To wipe functionality of superclass.

    private void FixedUpdate()
    {
        Vector3 moveDirection = speed * Time.fixedDeltaTime * direction;
        rb.MovePosition(transform.position + moveDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                {
                    other.GetComponent<ZombieAI>().Health -= damage;
                    goto case "Wall";
                }
            case "Barricade": // Damage barricade if intact, then act like wall
                {
                    if (other.GetComponent<Barricade>().IsBroken)
                    {
                        return;
                    }

                    other.GetComponent<Barricade>().DamageBarricade(damage);

                    goto case "Wall";
                }
            case "Wall": // Fall to ground and begin expanding.
                {
                    if (blownUp) break;
                    blownUp = true;
                    direction = Vector3.zero;
                    rb.useGravity = true;
                    rb.isKinematic = true;
                    emitter.Play();
                    StartCoroutine(Expand());

                    break;
                }
            default: // Assume bullet isn't meant to collide
                {
                    break;
                }
        }
    }

    /// <summary> Expand the bullet to the maximum size,
    /// then call Fade(). </summary>
    /// <returns> Unused. </returns>
    private IEnumerator Expand()
    {
        Vector3 originalScale = transform.localScale;

        float currentScale = originalScale.x;

        while (currentScale < expandSize)
        {
            currentScale = Mathf.Min(currentScale + Time.deltaTime * 500, expandSize);

            transform.localScale = currentScale * originalScale;

            yield return null;
        }

        StartCoroutine(Fade());
    }

    /// <summary> Fade out the bullet over a series of cycles until it is
    /// invisible, then destroy it. </summary>
    /// <param name="fadeAmount"> The percentage to fade by per cycle.</param>
    /// <param name="fadeWait"> The time per cycle. </param>
    /// <returns> Unused. </returns>
    private IEnumerator Fade(float fadeAmount = .1f, float fadeWait = .1f)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();

        for (
                float alpha = renderer.material.color.a;
                alpha >= 0;
                alpha -= fadeAmount
            )
        {
            Color c = renderer.material.color;
            c.a = alpha;
            renderer.material.color = c;

            yield return new WaitForSeconds(fadeWait);
        }

        // TODO: replace with object pooling

        Destroy(gameObject);
    }
}
