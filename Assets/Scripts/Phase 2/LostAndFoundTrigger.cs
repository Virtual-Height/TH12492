using UnityEngine;

public class LostAndFoundTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.instance.selectedAI != null)
            {
                GameManager.instance.submitButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.submitButton.SetActive(false);
        }
    }
}
