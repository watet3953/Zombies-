using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float lifetime = 5f;
    [SerializeField] protected float damage = 5f;

    protected Vector3 direction = Vector3.zero;

    protected void Start() { Destroy(gameObject, lifetime); }

    protected void Update() { transform.Translate(direction * speed * Time.deltaTime); }

    /// <summary>
    /// Sets the direction of the bullet to travel
    /// </summary>
    /// <param name="dir"> The direction for the bullet to travel in, global coordinates.</param>
    public void Fire(Vector3 dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barricade"))
        {
            other.GetComponent<Barricade>().DamageBarricade(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

    }
}
