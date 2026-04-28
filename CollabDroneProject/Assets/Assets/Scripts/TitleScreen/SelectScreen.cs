using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectScreen : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button TutorialButton;
    [SerializeField] private Button Level1Button;
    [SerializeField] private Button Level2Button;
    [SerializeField] private Button BackButton;

    [Header("Scene Settings")]
    [SerializeField] private string tutorialSceneName;
    [SerializeField] private string level1SceneName;
    [SerializeField] private string level2SceneName;
    [SerializeField] private string backSceneName;

    private void Start()
    {
        TutorialButton.onClick.AddListener(PlayTutorial);
        Level1Button.onClick.AddListener(PlayLevel1);
        Level2Button.onClick.AddListener(PlayLevel2);
        BackButton.onClick.AddListener(UseBackButton);
    }

    private void PlayTutorial()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    private void PlayLevel1()
    {
        SceneManager.LoadScene(level1SceneName);
    }

    private void PlayLevel2()
    {
        SceneManager.LoadScene(level2SceneName);
    }

    private void UseBackButton()
    {
        SceneManager.LoadScene(backSceneName);
    }
}