using System;
using UnityEngine;
using System.Collections;
using StarterAssets;



public class QuizOptionElement : MonoBehaviour
{
    [Header("Quiz Data")]
    public QuizeHolder[] questions;

    [Header("Popup & Audio Settings")]
    public GameObject infoPopup;        // Popup shown before quiz starts
    public AudioSource audioSource;     // Each object has its own AudioSource
    public AudioClip infoAudioClip;     // The clip for this specific quiz intro

    private bool isProcessing = false;




    /* private void OnMouseDown()
     {
         if (!isProcessing)
         {
             isProcessing = true;
             StartCoroutine(HandleQuizFlow());
         } 
     }*/

    private void OnTriggerEnter(Collider other)
    {
        // Make sure only player or a specific tag triggers it
        if (!isProcessing && other.CompareTag("Player"))
        {

            ThirdPersonController playerMoves = other.GetComponent<ThirdPersonController>();
            Animator playerAnimator = other.GetComponent<Animator>();

            if (playerMoves != null)
            {
                playerMoves.canMove = false;
            }

            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Speed", 0f);
                //playerAnimator.enabled = false;
            }

            isProcessing = true;
            //StartCoroutine(HandleQuizFlow());
           // StartCoroutine(HandleQuizFlow(playerMoves));
            StartCoroutine(HandleQuizFlow(playerMoves, playerAnimator));

        }

        else
        {
            Debug.Log("not work is call...");
        }
    }



    private IEnumerator HandleQuizFlow(ThirdPersonController playerMove ,Animator playerAnimator)
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



        QuizManager.instance.OnQuizComplete += () =>
        {
            if (playerMove != null)
                playerMove.canMove = true; 
        };

        if (playerAnimator != null)
            playerAnimator.enabled = true;


        isProcessing = false;
    }
}

[Serializable]
public class QuizeHolder
{
    public Sprite hinQuizImage;
    public QuizOptionHolder[] hinOptions;
}