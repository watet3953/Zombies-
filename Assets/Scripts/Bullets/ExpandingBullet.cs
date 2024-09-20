using System.Collections;
using UnityEngine;

public class ExpandingBullet : Bullet
{
    [SerializeField] private float expandSize = 5f;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private new void Start() { }

    private new void Update() { }

    private void FixedUpdate()
    {
        Vector3 moveDirection = direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + moveDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            direction = Vector3.zero;
            rb.useGravity = true;
            rb.isKinematic = true;

            StartCoroutine(Expand());
        }
    }

    private IEnumerator Expand()
    {
        Vector3 originalScale = transform.localScale;

        float currentScale = originalScale.x;

        while (currentScale < expandSize)
        {
            currentScale = Mathf.MoveTowards(currentScale, expandSize, Time.deltaTime);
            transform.localScale = currentScale * Vector3.one;
            yield return null;
        }

        StartCoroutine(Fade());
    }

    private IEnumerator Fade(float fadeAmount = .05f, float fadeWait = 1f)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        float alpha = renderer.material.color.a;

        for (; alpha >= 0; alpha -= fadeAmount)
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
