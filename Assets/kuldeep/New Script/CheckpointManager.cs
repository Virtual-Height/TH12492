using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public GameObject[] checkpointLocations;   // Assign GameObjects with BoxColliders here
    public GameObject checkpointVisual;        // Optional visual marker (can follow active location)

    private int currentCheckpointIndex = 0;

    private UIManager ui;

    public GameObject harHarAfterPlayerPos;

    void Start()
    {
        ui = UIManager.Instance;
  
        if (this.checkpointLocations.Length == 0)
        {
            Debug.LogError("CheckpointManager: No checkpoint locations assigned.");
            return;
        }

        // Initialize all checkpoint colliders to inactive
        for (int i = 0; i < this.checkpointLocations.Length; i++)
        {
            this.checkpointLocations[i].SetActive(false);
        }

        // Activate the first checkpoint
        ActivateCheckpoint(currentCheckpointIndex);

    }

    // Call this when the player completes the current activity
    public void CompleteCurrentActivity()
    {

        if (ui != null)
            ui.OnGhatCheckpointCompleted(currentCheckpointIndex);

        Debug.Log("CompleteCurrentActivity is calll ............");
        // Deactivate current
        this.checkpointLocations[currentCheckpointIndex].SetActive(false);

        currentCheckpointIndex++;

        if (currentCheckpointIndex < checkpointLocations.Length)
        {
            this.ActivateCheckpoint(currentCheckpointIndex);
        }
        else
        {
            Debug.Log("All checkpoints completed.");
            if (checkpointVisual != null)
                checkpointVisual.SetActive(false);

            ui.OnAllGhatActivitiesCompleted();
        }

    }

    private void ActivateCheckpoint(int index)
    {
        this.checkpointLocations[index].SetActive(true);

        if (checkpointVisual != null)
        {
            // Move visual to match the active checkpoint
            this.checkpointVisual.transform.position = this.checkpointLocations[index].transform.position;
        }

        Debug.Log("Activated checkpoint: " + checkpointLocations[index].name);
    }

}