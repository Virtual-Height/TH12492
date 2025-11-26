using UnityEngine;

public class GarbageClickable : MonoBehaviour
{
    private CharacterUjjainController player;

    private void Start()
    {
       // player = FindObjectOfType<CharacterUjjainController>();
    }

    private void OnMouseDown()
    {
        player = FindObjectOfType<CharacterUjjainController>();
        Debug.Log("OnMouseDown is call...");

        UIManager.Instance.ShowGarbageMessageOnce();

        if (player != null)
        {
            Debug.Log("Garbage clicked!");
            player.TriggerGarbageCollect(transform);
        }
        else
        {
            Debug.Log("Garbage not clicked!");
        }
    }
}