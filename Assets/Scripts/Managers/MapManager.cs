using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Singleton

    public static MapManager Instance
    {
        get;
        protected set;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("MapManager already exists, destroying old MapManager...");
            Destroy(Instance);
        }
        Instance = this;
    }
    #endregion Singleton

    [SerializeField] public ZombiePool pool;

    [SerializeField] public AudioClip music;

    float spawnDelta = 0f;
    [SerializeField] float spawnTime = 10f;

    private void Update()
    {
        spawnDelta -= Time.deltaTime;
        while (spawnDelta < 0)
        {
            spawnDelta += spawnTime;
            SpawnZombie(transform.position);
        }
    }


    private void SpawnZombie(Vector3 location)
    {
        location += new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));
        ZombieAI zombie = pool.Get(location);
    }
}
