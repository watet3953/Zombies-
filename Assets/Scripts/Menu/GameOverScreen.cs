using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void PlayAgain()
    {
        return; // i don't have time to write player health & damage sorry
    }

    public void DontPlayAgain()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
