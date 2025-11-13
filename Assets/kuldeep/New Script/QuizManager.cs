using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;
    public GameObject quizPannel;
    public Image quizImage;
    public QuizOption[] quizOptions;
    public List<QuizeHolder> questionsList;

    public event Action OnQuizComplete;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       
    }

    public void SetupQuestions(QuizeHolder[] quiestions)
    {
        questionsList = quiestions.ToList();
        SetupQuiz();
        quizPannel.SetActive(true);
    }

    public void SetupQuiz()
    {
        if (questionsList.Count > 0)
        {
            quizImage.sprite = questionsList[0].hinQuizImage;

            for (int i = 0; i < quizOptions.Length; i++)
            {
                quizOptions[i].btn.interactable = false;
            }

            StartCoroutine(StartQuestion());
            quizImage.sprite = questionsList[0].hinQuizImage;

            for (int i = 0; i < quizOptions.Length; i++)
            {
                quizOptions[i].btn.interactable = true;
                quizOptions[i].btn.image.color = Color.white;
                quizOptions[i].btn.transform.GetChild(1).GetComponent<Image>().sprite = questionsList[0].hinOptions[i].image;
                quizOptions[i].isCorrect = questionsList[0].hinOptions[i].isCorrect;
            }

            questionsList.RemoveAt(0);

        }
        else
        {
            StartCoroutine(CloseQuizPannel());
        }


    }
    public void OnOptionClick(int optionIndex)
    {
        Debug.Log("Selected Index :" + optionIndex);

        for (int i = 0; i < quizOptions.Length; i++)
        {
            if (quizOptions[i].btn != quizOptions[optionIndex].btn)
            {
                quizOptions[i].btn.interactable = false;
            }
        }

        if (quizOptions[optionIndex].isCorrect)
        {
            quizOptions[optionIndex].btn.image.color = Color.green;

            Debug.Log("answer true is call.. ");
            GameManager.instance.AddPoint(10);
        }
        else
        {
            for (int i = 0; i < quizOptions.Length; i++)
            {
                if (quizOptions[i].isCorrect)
                {
                    quizOptions[i].btn.image.color = Color.green;
                }
            }
            quizOptions[optionIndex].btn.image.color = Color.red;
        }

        StartCoroutine(NextQuestion());
    }

    IEnumerator StartQuestion()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < quizOptions.Length; i++)
        {
            quizOptions[i].btn.interactable = true;
        }
    }

    IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(3);
        SetupQuiz();
    }

    IEnumerator CloseQuizPannel()
    {
        yield return new WaitForSeconds(3);
        quizPannel.SetActive(false);
        OnQuizComplete?.Invoke();
    }
}

[Serializable]
public class QuizOption
{
    public Button btn;
    public bool isCorrect = false;
}

[Serializable]
public class QuizOptionHolder
{
    public Sprite image;
    public bool isCorrect = false;
}