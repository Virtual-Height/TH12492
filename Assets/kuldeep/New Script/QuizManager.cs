using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;

    [Header("Quiz UI")]
    public GameObject quizPanel;
    public Text questionText;
    public QuizOption[] quizOptions;

    [Header("Questions Data")]
    public List<QuizeHolder> questionsList;

    private void Awake()
    {
        instance = this;
    }

    public void SetupQuestions(QuizeHolder[] questions)
    {
        questionsList = questions.ToList();
        SetupQuiz();
        quizPanel.SetActive(true);
    }

    public void SetupQuiz()
    {
        if (questionsList.Count > 0)
        {
            // Set question text
            questionText.text = questionsList[0].questionText;

            // Disable buttons initially
            for (int i = 0; i < quizOptions.Length; i++)
            {
                quizOptions[i].btn.interactable = false;
            }

            StartCoroutine(StartQuestion());

            // Assign option texts and correctness
            for (int i = 0; i < quizOptions.Length; i++)
            {
                quizOptions[i].btn.interactable = true;
                quizOptions[i].btn.image.color = Color.white;
                quizOptions[i].optionText.text = questionsList[0].options[i].optionText;
                quizOptions[i].isCorrect = questionsList[0].options[i].isCorrect;
            }

            // Remove the used question
            questionsList.RemoveAt(0);
        }
        else
        {
            StartCoroutine(CloseQuizPanel());
        }
    }

    public void OnOptionClick(int optionIndex)
    {
        Debug.Log("Selected Index: " + optionIndex);

        // Disable other buttons
        for (int i = 0; i < quizOptions.Length; i++)
        {
            if (quizOptions[i].btn != quizOptions[optionIndex].btn)
            {
                quizOptions[i].btn.interactable = false;
            }
        }

        // Check answer
        if (quizOptions[optionIndex].isCorrect)
        {
            quizOptions[optionIndex].btn.image.color = Color.green;
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

    IEnumerator CloseQuizPanel()
    {
        yield return new WaitForSeconds(3);
        quizPanel.SetActive(false);
    }
}

[Serializable]
public class QuizOption
{
    public Button btn;
    public Text optionText;
    public bool isCorrect = false;
}
