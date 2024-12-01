using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        // ask game manager to load level 1
        GameManager.Instance.StartNewGame();
        // wipe save
    }

    public void Continue()
    {
        // ask game manager to load save data
    }

    public void Options()
    {
        // implement on your own
    }

    public void Exit()
    {
#if UNITY_STANDALONE
        Application.Quit();
 #endif
    #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
