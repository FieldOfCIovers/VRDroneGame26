using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RaceManager : MonoBehaviour
{
    public Ring[] rings;

    public GameObject StartUI;
    private bool raceActive = false;

    public float timeRemaining = 200f;
    public TMP_Text timerText;

    public GameObject winUI;
    public GameObject loseUI;

    private int currentRing = 0;
    

    void Start()
    {
        Debug.Log("Started race");
        Debug.Log("Activating first ring: " + rings[0].name);
        // Hide all rings first
        foreach (var ring in rings)
            ring.SetVisible(false);

        Debug.Log("Trying to activate first ring");

        // Activate first ring
        //rings[0].SetVisible(true);
        StartUI.SetActive(true);
    }
    public void StartEasy()
    {
        timeRemaining = 200f;
        StartRace();
    }

    public void StartHard()
    {
        timeRemaining = 90f;
        StartRace();
    }

    void StartRace()
    {
        StartUI.SetActive(false);

        raceActive = true;

        // Activate first ring
        rings[0].SetVisible(true);
    }
    void Update()
    {
        if (!raceActive) return;

        // TIMER
        timeRemaining -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString();

        if (timeRemaining <= 0)
        {
            Lose();
        }
    }

    public void RingPassed(int index)
    {
        if (!raceActive) return;

        if (index != currentRing) return;

        // Hide current
        rings[currentRing].SetVisible(false);

        currentRing++;

        // All rings done
        if (currentRing >= rings.Length)
        {
            Win();
            return;
        }

        // Show next
        rings[currentRing].SetVisible(true);
    }

    void Win()
    {
        raceActive = false;
        winUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void Lose()
    {
        raceActive = false;
        loseUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
