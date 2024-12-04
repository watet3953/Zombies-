using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    public bool canPause = false;
    bool isPaused = false;

    public void OnPauseGame(InputValue inputValue)
    {
        Pause(!isPaused);
    }

    private void Pause(bool pause)
    {
        if (canPause || !pause) // safety catch in case player is paused when going into a spot where they shouldn't be able to.
        {
            isPaused = pause;
            canvas.SetActive(isPaused);

            GameManager.Instance.Player.Body.enabled = !isPaused;
            GameManager.Instance.Player.enabled = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }
}
