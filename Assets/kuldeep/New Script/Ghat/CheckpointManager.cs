using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public GameObject[] checkpointLocations;   // Assign GameObjects with BoxColliders here
    public GameObject checkpointVisual;        // Optional visual marker (can follow active location)

    private int currentCheckpointIndex = 0;

    void Start()
    {
        if (checkpointLocations.Length == 0)
        {
            Debug.LogError("CheckpointManager: No checkpoint locations assigned.");
            return;
        }

        // Initialize all checkpoint colliders to inactive
        for (int i = 0; i < checkpointLocations.Length; i++)
        {
            checkpointLocations[i].SetActive(false);
        }

        // Activate the first checkpoint
        ActivateCheckpoint(currentCheckpointIndex);
    }

    // Call this when the player completes the current activity
    public void CompleteCurrentActivity()
    {

        Debug.Log("CompleteCurrentActivity is calll ............");
        // Deactivate current
        checkpointLocations[currentCheckpointIndex].SetActive(false);

        currentCheckpointIndex++;

        if (currentCheckpointIndex < checkpointLocations.Length)
        {
            ActivateCheckpoint(currentCheckpointIndex);
        }
        else
        {
            Debug.Log("All checkpoints completed.");
            if (checkpointVisual != null)
                checkpointVisual.SetActive(false);
        }
    }

    private void ActivateCheckpoint(int index)
    {
        checkpointLocations[index].SetActive(true);

        if (checkpointVisual != null)
        {
            // Move visual to match the active checkpoint
            checkpointVisual.transform.position = checkpointLocations[index].transform.position;
        }

        Debug.Log("Activated checkpoint: " + checkpointLocations[index].name);
    }
}
