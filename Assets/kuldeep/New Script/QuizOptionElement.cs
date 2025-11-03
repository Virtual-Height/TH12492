using System;
using UnityEngine;

public class QuizOptionElement : MonoBehaviour
{
    public QuizeHolder[] questions;

    private void OnMouseDown()
    {
        QuizManager.instance.SetupQuestions(questions);
    }
}

[Serializable]
public class QuizeHolder
{
    [TextArea(2, 5)]
    public string questionText;

    public QuizOptionHolder[] options;
}

[Serializable]
public class QuizOptionHolder
{
    public string optionText;
    public bool isCorrect = false;
}
