using System;
using UnityEngine;
using System.Collections;

public class QuizOptionElement : MonoBehaviour
{
    [Header("Quiz Data")]
    public QuizeHolder[] questions;

    [Header("Popup & Audio Settings")]
    public GameObject infoPopup;        // Popup shown before quiz starts
    public AudioSource audioSource;     // Each object has its own AudioSource
    public AudioClip infoAudioClip;     // The clip for this specific quiz intro

    private bool isProcessing = false;

    private void OnMouseDown()
    {
        if (!isProcessing)
        {
            isProcessing = true;
            StartCoroutine(HandleQuizFlow());
        }
    }

    private IEnumerator HandleQuizFlow()
    {
        // Step 1: Show info popup
        if (infoPopup != null)
            infoPopup.SetActive(true);

        // Step 2: Play assigned audio (if any)
        float waitTime = 2f; // default wait if no audio

        if (audioSource != null && infoAudioClip != null)
        {
            audioSource.clip = infoAudioClip;
            audioSource.Play();
            waitTime = infoAudioClip.length;
        }

        yield return new WaitForSeconds(waitTime);

        // Step 3: Hide popup after audio finishes
        if (infoPopup != null)
            infoPopup.SetActive(false);

        QuizManager.instance.SetupQuestions(questions);
        isProcessing = false;
    }
}

[Serializable]
public class QuizeHolder
{
    public Sprite hinQuizImage;
    public QuizOptionHolder[] hinOptions;
}