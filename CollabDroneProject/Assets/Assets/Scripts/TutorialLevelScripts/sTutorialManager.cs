using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class sTutorialManager : MonoBehaviour
{
    //Object Pool Variables
    public static sTutorialManager SharedInstance;
    public List<GameObject> prefabsToPool;
    public List<GameObject> pooledObjects;

    public int amountToPool;
    public Transform spawnPoint;
    public Transform droneSpawnPoint;
    public GameObject drone;

    GameObject starterCourse;
    GameObject secondCourse;
    private bool courseSpawned = false;
    private int currentStage = 0;


    private sRingCollisionScript[] collisionScripts;

    //Dialogue Check Variables
    private List<int> keyList;
    private bool firstCheck = false;
    private int secondStage = 0;
    public DialogueTrigger[] DialogueTriggers;
    public DialogueManager DialogueManager;

    //Textbox Check Variables
    public List<TextMeshProUGUI> checklistGUI;
    public float colorFadeDuration = 0.5f;
    public Animator[] checklistAnimators;
    private int activeCheck = -1;
    private bool checklistStarted = false;

    //Fade To Black Variables 
    public CanvasGroup fadePanel;
    public float fadeDuration;
    private bool isFading = false;


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void objPoolStart()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        foreach(GameObject prefab in prefabsToPool)
        {
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(prefab);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
    }

    private GameObject GetObjectsPooled(int prefabIndex)
    {
        Debug.Log("prefabIndex received: " + prefabIndex + " amountToPool: " + amountToPool);

        int start = prefabIndex * amountToPool;
        int end = start + amountToPool;

        Debug.Log("Searching indexes " + start + " to " + end);
        for (int i = start; i < end; i++)
        {
            Debug.Log("Index " + i + " active: " + pooledObjects[i].activeInHierarchy);
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    private void ReturnObject(GameObject obj)
    {

        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        objPoolStart();

        StartCoroutine(FadeFromBlack());
        //Start Code for Dialogue in Tutorial
        keyList = new List<int>();
        setActiveChecklist(-1);
        TriggerDialogueStage();
    }

    // Update is called once per frame
    void Update()
    {
        listenForInput();

        if (DialogueManager.ReturnCounter() == 4 && currentStage == 0)
        {
            starterCourse = GetObjectsPooled(0);
            if (starterCourse != null && courseSpawned == false)
            {
                //courseSpawned = true;
                currentStage = 1;
                StartCoroutine(SpawnCourse(starterCourse));
            }
        }

        if (DialogueManager.ReturnCounter() == 4 && currentStage == 1)
        {
            if (collisionScripts != null && collisionScripts.Length > 0)
            {
                foreach (sRingCollisionScript script in collisionScripts)
                {
                    if (script.finishedCourse == true)
                    {
                        script.finishedCourse = false;
                        currentStage = 2;
                        StartCoroutine(ReturnCourse(starterCourse));
                    }
                }
            }
        }

        if (DialogueManager.ReturnCounter() == 5 && currentStage == 2)
        {
            Debug.Log("Trying to get second course, pool count: " + pooledObjects.Count);
            secondCourse = GetObjectsPooled(1);
            Debug.Log("secondCourse is: " + secondCourse);
            if (secondCourse != null && courseSpawned == false)
            {
                //courseSpawned = true;
                currentStage = 3;
                StartCoroutine(SpawnCourse(secondCourse));
            }
            else
            {
                Debug.Log("GetObjectsPooled(1) returned null");
            }
        }
        if (DialogueManager.ReturnCounter() == 5 && currentStage == 3)
        {
            if (collisionScripts != null && collisionScripts.Length > 0)
            {
                foreach (sRingCollisionScript script in collisionScripts)
                {
                    if (script.finishedCourse == true)
                    {
                        script.finishedCourse = false;
                        currentStage = 4;
                        StartCoroutine(ReturnCourse(secondCourse));
                    }
                }
            }
        }

        if (DialogueManager.ReturnCounter() == 6 && currentStage == 4)
        {
            currentStage = 5;
            StartCoroutine(FadeToBlack());
            SceneManager.LoadScene("TitleScreen");
        }
    }

    private IEnumerator SpawnCourse(GameObject courseType)
    {
        yield return StartCoroutine(FadeToBlack());

        drone.transform.position = droneSpawnPoint.position;
        drone.transform.rotation = droneSpawnPoint.rotation;
        courseType.transform.position = spawnPoint.position;
        courseType.transform.rotation = spawnPoint.rotation;
        courseType.SetActive(true);
        collisionScripts = FindObjectsByType<sRingCollisionScript>(FindObjectsSortMode.None);

        yield return StartCoroutine(FadeFromBlack());
    }

    private IEnumerator ReturnCourse(GameObject courseType)
    {
        yield return StartCoroutine(FadeToBlack());

        drone.transform.position = droneSpawnPoint.position;
        drone.transform.rotation = droneSpawnPoint.rotation;
        ReturnObject(courseType);
        
        TriggerDialogueStage();
        courseSpawned = false;

        yield return StartCoroutine(FadeFromBlack());
        Debug.Log("Returning: " + courseType.name);
    }


    void countSecondStage(int phase)
    {
        secondStage = phase;
    }

    void TriggerDialogueStage()
    {
        int stage = DialogueManager.ReturnCounter();

        if (stage<DialogueTriggers.Length && DialogueTriggers[stage] != null)
        {
            DialogueTriggers[stage].TriggerDialogue();
        }
        else
        {
            Debug.Log("Tutorial Complete, No More Dialogue");
        }
    }

    private IEnumerator FadeTextColor(TMP_Text text, Color targetColor, float duration)
    {
        if (text == null)
        {
            yield break;
        }

        Color startColor = text.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.color = Color.Lerp(startColor, targetColor, elapsed / duration);
            yield return null;
        }
        text.color = targetColor;
    }

    private void setActiveChecklist(int index)
    {
        for (int i = 0; i < checklistAnimators.Length; i++)
        {
            checklistAnimators[i].SetBool("checkOpen", false);
        }
        if (index >= 0 && index < checklistAnimators.Length)
        {
            checklistAnimators[index].SetBool("checkOpen", true);
            activeCheck = index;
        }

    }

    private void listenForInput()
    {
        if (Input.GetKeyUp(KeyCode.X) || Input.GetButton("Joystick Button 0"))
        {
            DialogueManager.DisplayNextSentence();
        }

        if (DialogueManager.ReturnCounter() == 1 && firstCheck != true)
        {
            if (checklistStarted != true)
            {
                setActiveChecklist(0);
                checklistStarted = true;
            }


            if (Input.GetKeyUp(KeyCode.W) || Input.GetAxis("Vertical") > 0)
            {
                StartCoroutine(FadeTextColor(checklistGUI[0], Color.darkGreen, colorFadeDuration));
                keyList.Add(0);
            }

            if (Input.GetKeyUp(KeyCode.S) || Input.GetAxis("Vertical") < 0)
            {
                StartCoroutine(FadeTextColor(checklistGUI[1], Color.darkGreen, colorFadeDuration));
                keyList.Add(1);
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetAxis("Vertical") > 0)
            {
                StartCoroutine(FadeTextColor(checklistGUI[2], Color.darkGreen, colorFadeDuration));
                keyList.Add(2);
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetAxis("Vertical") < 0)
            {
                StartCoroutine(FadeTextColor(checklistGUI[3], Color.darkGreen, colorFadeDuration));
                keyList.Add(3);
            }

            if (new[] { 0,1,2,3 }.All(keyList.Contains))
            {
                keyList.Clear();
                checklistStarted = false;
                setActiveChecklist(-1);
                Debug.Log("Ending First Check");
                firstCheck = true;
                countSecondStage(1);
                TriggerDialogueStage();
            }
        }

        else if (secondStage == 1 && DialogueManager.ReturnCounter() == 2)
        {
            if (checklistStarted != true)
            {
                setActiveChecklist(1);
                checklistStarted = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetAxis("Joystick Axis 10") < 0)
            {
                StartCoroutine(FadeTextColor(checklistGUI[4], Color.darkGreen, colorFadeDuration));
                keyList.Add(0);
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetAxis("Joystick Axis 9") < 0)
            {
                StartCoroutine(FadeTextColor(checklistGUI[5], Color.darkGreen, colorFadeDuration));
                keyList.Add(1);
            }

            if (new[] { 0, 1}.All(keyList.Contains))
            {
                keyList.Clear();
                checklistStarted = false;
                setActiveChecklist(-1);
                Debug.Log("Ending First Check");
                countSecondStage(2);
                TriggerDialogueStage();
            }
        }

        else if (secondStage == 2 && DialogueManager.ReturnCounter() == 3)
        {
            if (checklistStarted != true)
            {
                setActiveChecklist(2);
                checklistStarted = true;
            }

            if (Input.GetKeyUp(KeyCode.C) || Input.GetButton("Joystick Button 3"))
            {
                StartCoroutine(FadeTextColor(checklistGUI[6], Color.darkGreen, colorFadeDuration));
                keyList.Add(0);
            }

            if (new[] {0}.All(keyList.Contains))
            {
                keyList.Clear();
                checklistStarted = false;
                setActiveChecklist(-1);
                Debug.Log("Ending First Check");
                countSecondStage(3);
                TriggerDialogueStage();
            }
        }
    }

    private IEnumerator FadeToBlack()
    {

        fadePanel.alpha = 0.0f;
        float elapsed = 0.0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0.0f, 1.0f, elapsed / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 1.0f;
    }

    private IEnumerator FadeFromBlack()
    {

        fadePanel.alpha = 1.0f;
        float elapsed = 0.0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1.0f, 0.0f, elapsed / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 0.0f;
    }

}


//Ring Test 1:
//-213.4 / 8.2 / -82