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

    [SerializeField] private ZombieAI zombiePrefab;

    float spawnDelta = 0f;
    readonly float spawnTime = 10f;

    private void Update()
    {
        spawnDelta -= Time.deltaTime;
        while (spawnDelta < 0)
        {
            spawnDelta += spawnTime;
            SpawnZombie(transform); // TEMP
        }
    }


    private void SpawnZombie(Transform location)
    {
        ZombieAI zombie = Instantiate(zombiePrefab.gameObject, location).GetComponent<ZombieAI>();
        zombie.player = GameManager.Instance.Player.Body;
    }
}
