using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance;
    public static GameManager Instance 
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<GameManager>(); // final try before failing
            if (!Instance)
                throw new System.Exception("No GameManager Instance Found.");
            return instance;
        }
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // make gamemanager persist
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("Could not create GameManager, GameManager already exists.");
        }
    }
    #endregion Singleton

    [SerializeField] PlayerController player;
    public PlayerController Player => player;

    private void Start()
    {
        Debug.Assert(player != null, "Player is not present in Persistent Scene.");

        StartCoroutine(LoadMap(levelNames[0]));
    }

    #region SceneManagement

    bool curLoading = false;
    string curLevelName;

    public string[] levelNames;

    IEnumerator LoadMap(string levelName)
    {
        Debug.Assert(!curLoading, "Cannot load new map while already loading another.");
        curLoading = true;
        player.gameObject.SetActive(false);

        // unload all non-persistent scenes.
        if (!string.IsNullOrEmpty(curLevelName))
            yield return SceneManager.UnloadSceneAsync(curLevelName);

        // load desired level.
        yield return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        curLevelName = levelName;

        // Reset non-persistent data.
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));
        player.Body.transform.position = Vector3.up * 5;
        player.gameObject.SetActive(true);
        curLoading = false;
        yield break;
    }

    #endregion SceneManagement



}
