using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Slider health, enemiesLeft;

    PlayerBody body;

    private void OnEnable()
    {
        body = GameManager.Instance.Player.Body;

        UpdatePlayerHealth();
    }

    private void UpdatePlayerHealth()
    {
        health.value = 0f; //TODO: add player health
    }

    private void UpdateEnemiesLeft()
    {
        enemiesLeft.value = 1f - ((float)MapManager.Instance.pool.available.Count / MapManager.Instance.pool.pool.Count);
    }

    private void Update()
    {
        UpdatePlayerHealth();
        UpdateEnemiesLeft();
    }
}
