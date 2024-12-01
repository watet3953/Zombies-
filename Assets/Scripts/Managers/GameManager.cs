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
            if (!instance)
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

        player.enabled = false;
        player.Body.enabled = false;
        StartCoroutine(LoadMap(mainMenuName));
    }

    #region Menu Management

    [SerializeField] string mainMenuName = "MainMenu";
    public void StartNewGame()
    {
        player.enabled = true;
        player.Body.enabled = true;
        StartCoroutine(LoadMap(startingMapName));
    }

    #endregion Menu Mangement

    #region SceneManagement

    bool curLoading = false;
    string curMapName;

    public string startingMapName;

    public IEnumerator LoadMap(string mapName)
    {
        if (curLoading)
        {
            Debug.LogError("Cannot load new map while already loading another.");
            yield break;
        }
        curLoading = true;
        player.gameObject.SetActive(false);

        // unload all non-persistent scenes.
        yield return AudioManager.Instance.StartCoroutine(AudioManager.Instance.FadeOut());
        if (!string.IsNullOrEmpty(curMapName))
            yield return SceneManager.UnloadSceneAsync(curMapName);

        // load desired level.
        yield return SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive);
        curMapName = mapName;

        // Reset non-persistent data.
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapName));
        player.Body.transform.position = Vector3.up * 5; //FIXME: replace with actual spawn position.
        player.gameObject.SetActive(true);

        AudioManager.Instance.LoadLevelComplete();

        curLoading = false;
        yield break;
    }

    #endregion SceneManagement



}
