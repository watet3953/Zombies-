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

        LoadMainMenu();
    }

    #region Menu Management

    [SerializeField] string mainMenuName = "MainMenu";
    [SerializeField] LoadScreen loadScreen;
    [SerializeField] PauseScreen pauseScreen;
    [SerializeField] HUD hud;

    public void LoadMainMenu()
    {
        saveData = SaveHandler.Load();
        player.enabled = false;
        player.Body.enabled = false;
        StartCoroutine(LoadMap(mainMenuName, false));
    }

    public void StartNewGame()
    {
        saveData = new(mainMenuName);
        player.enabled = true;
        player.Body.enabled = true;
        StartCoroutine(LoadMap(startingMapName, true));
    }

    public void LoadGame()
    {
        if (SaveHandler.SavePresent)
        {
            player.enabled = true;
            player.Body.enabled = true;
            StartCoroutine(LoadMap(saveData.curMap, true));
        }
    }

    #endregion Menu Mangement

    #region SceneManagement

    bool curLoading = false;

    public string startingMapName;
    public SaveData saveData;

    public IEnumerator LoadMap(string mapName, bool gameplayMap)
    {
        if (curLoading)
        {
            Debug.LogError("Cannot load new map while already loading another.");
            yield break;
        }
        pauseScreen.canPause = false;
        hud.enabled = false;
        loadScreen.gameObject.SetActive(true);
        yield return loadScreen.StartCoroutine(loadScreen.FakeLoad(1f));
        curLoading = true;
        player.gameObject.SetActive(false);

        // unload all non-persistent scenes.
        yield return AudioManager.Instance.StartCoroutine(AudioManager.Instance.FadeOut());
        if (!string.IsNullOrEmpty(saveData.curMap) && SceneManager.GetSceneByName(saveData.curMap).isLoaded)
            yield return SceneManager.UnloadSceneAsync(saveData.curMap);

        // load desired level.
        yield return SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive);
        saveData.curMap = mapName;

        // Reset non-persistent data.
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapName));
        player.Body.transform.position = Vector3.up * 5; //FIXME: replace with actual spawn position.
        player.gameObject.SetActive(true);

        loadScreen.gameObject.SetActive(false);
        AudioManager.Instance.LoadLevelComplete();

        pauseScreen.canPause = gameplayMap;
        hud.enabled = gameplayMap;

        curLoading = false;
        yield break;
    }

    #endregion SceneManagement
}
