using System;
using UnityEngine;
using System.Collections;
using StarterAssets;



public class QuizOptionElement : MonoBehaviour
{
    [Header("Quiz Data")]
    public QuizeHolder[] questions;

    [Header("Popup & Audio Settings")]
    public GameObject infoPopup;        
    public AudioSource audioSource;     
    public AudioClip infoAudioClip;    

    private bool isProcessing = false;

    private void OnTriggerEnter(Collider other)
    {
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
            }

            isProcessing = true;
            StartCoroutine(HandleQuizFlow(playerMoves, playerAnimator));
        }
        else
        {
            Debug.Log("not work is call...");
        }
    }
    private IEnumerator HandleQuizFlow(ThirdPersonController playerMove ,Animator playerAnimator)
    {
        if (infoPopup != null)
            infoPopup.SetActive(true);

        float waitTime = 2f; 

        if (audioSource != null && infoAudioClip != null)
        {
            audioSource.clip = infoAudioClip;
            audioSource.Play();
            waitTime = infoAudioClip.length;
        }

        yield return new WaitForSeconds(waitTime);
      
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