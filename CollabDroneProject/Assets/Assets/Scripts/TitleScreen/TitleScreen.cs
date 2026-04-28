using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button fullscreenButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Settings")]
    [SerializeField] private string playSceneName = "TitleScreen";

    private bool isFullscreen;

    private void Start()
    {
        // Set initial fullscreen state
        isFullscreen = Screen.fullScreen;

        // Assign button listeners
        playButton.onClick.AddListener(PlayGame);
        fullscreenButton.onClick.AddListener(ToggleFullscreen);
        quitButton.onClick.AddListener(QuitGame);
    }

    // PLAY BUTTON
    private void PlayGame()
    {
        SceneManager.LoadScene(playSceneName);
    }

    // FULLSCREEN BUTTON
    private void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;

        Debug.Log("Fullscreen: " + isFullscreen);
    }

    // QUIT BUTTON
    private void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}