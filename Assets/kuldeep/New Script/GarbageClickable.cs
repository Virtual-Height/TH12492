using UnityEngine;

public class GarbageClickable : MonoBehaviour
{
    private CharacterUjjainController player;

    private void Start()
    {
        player = FindObjectOfType<CharacterUjjainController>();
    }

    private void OnMouseDown()
    {
        if (player != null)
        {
            Debug.Log("Garbage clicked!");
            player.TriggerGarbageCollect(transform);
        }
    }
}
