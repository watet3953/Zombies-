﻿using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary> The velocity of the bullet in units per second. </summary>
    [SerializeField] protected float speed = 5f;

    /// <summary> How long the bullet will exist in seconds. </summary>
    [SerializeField] protected float lifetime = 5f;

    /// <summary> The damage the bullet will do upon impact. </summary>
    [SerializeField] protected float damage = 5f;

    [SerializeField] protected ParticleSystem impactParticles;
    [SerializeField] protected AudioSource impactSound;

    /// <summary> The direction of travel as a normalized vector. </summary>
    protected Vector3 direction = Vector3.zero;


    protected void Start()
    {
        Destroy(gameObject, lifetime);
    }

    protected void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    /// <summary> Sets the direction of the bullet to travel. </summary>
    /// <param name="dir"> The direction of travel, global coordinates.</param>
    public void Fire(Vector3 dir)
    {
        Debug.Assert(
                Mathf.Abs(dir.magnitude - 1.0f) <= 0.1f,
                "Bullet fire direction is not normalized."
            );
        direction = dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                {
                    other.GetComponent<ZombieAI>().Health -= damage;
                    StartCoroutine(Hit());
                    break;
                }
            case "Barricade": // Damage barricade if intact, then act like wall
                {
                    if (other.GetComponent<Barricade>().IsBroken)
                    {
                        break;
                    }
                    other.GetComponent<Barricade>().DamageBarricade(damage);
                    goto case "Wall";
                }
            case "Wall": // Destroy the bullet
                {
                    StartCoroutine(Hit());
                    break;
                }
            default: // Assume bullet isn't meant to collide
                {
                    break;
                }
        }
    }

    private IEnumerator Hit()
    {
        speed = 0;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        impactParticles.Play();
        impactSound.Play();
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => impactSound.time >= impactSound.clip.length); // wait for sound to finish playing (my wonderful lambdas)
        Destroy(gameObject);
    }
}
