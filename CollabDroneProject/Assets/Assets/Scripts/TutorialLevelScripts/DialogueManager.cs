using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    
    private int dialogueCounter = 0;
    private Queue<string> sentences;
    public TextMeshProUGUI dialogueText;
    private bool endedAlready = false;

    public Animator animator;
    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting Teaching UI");

        animator.SetBool("isOpen", true);
        endedAlready = false;

        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0 && endedAlready !=true)
        {
            EndDialogue();
            return;
        }

        if (endedAlready == true)
        {
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        Debug.Log(sentence);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    public void EndDialogue() {
        Debug.Log("Dialogue End");
        endedAlready = true;
        animator.SetBool("isOpen", false);
        dialogueCounter++;
        Debug.Log(dialogueCounter);
    }

    public int ReturnCounter()
    {
        return dialogueCounter;
    }

}
