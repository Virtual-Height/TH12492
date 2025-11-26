using UnityEngine;

public class QuizZoneTrigger : MonoBehaviour
{
    public string quizID; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.activeQuiz = quizID;
        }
    }
}
