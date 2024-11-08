using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        { // lazy singleton.
            Debug.LogError("Multiple GameManagers Detected, destroying old manager.");
            Destroy(instance);
            return;
        }
        Debug.Log("Initializing GameManager");
        instance = this;
    }


}
