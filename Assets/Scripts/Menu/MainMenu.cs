using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    [SerializeField] Button continueButton;

    public void Start()
    {
        continueButton.interactable = SaveHandler.SavePresent;
    }

    public void NewGame()
    {
        // ask game manager to load level 1
        GameManager.Instance.StartNewGame();
        // wipe save
    }

    public void Continue()
    {
        GameManager.Instance.LoadGame();
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
    }
    public void ExitOptions()
    {
        optionsMenu.SetActive(false);
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

    #region Options Menu
    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    public void OnMasterVolumeChanged() => AudioManager.Instance.SetMasterVolume(MasterVolumeSlider.value);
    public void OnSFXVolumeChanged() => AudioManager.Instance.SetSFXVolume(SFXVolumeSlider.value);
    public void OnMusicVolumeChanged() => AudioManager.Instance.SetMusicVolume(MusicVolumeSlider.value);

    #endregion Options Menu
}
