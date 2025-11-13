using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GhatInstructionManager : MonoBehaviour
{
    public static GhatInstructionManager Instance;

    [Header("Instruction UI")]
    public GameObject ghatInstructionScreen;
    public Text instructionText;

    [Header("Timing")]
    public float messageDelay = 2f;   // Delay between different messages
    public float popupVisibleTime = 3f; // How long the popup stays visible

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.layer == LayerMask.NameToLayer("Ghat"))
        {
            Debug.Log("GhatInstruction Manager triggered...");
            StartCoroutine(InstructionSequence());
        }
    }

    private IEnumerator InstructionSequence()
    {
        // 1️⃣ Message 1
        ShowMessage("Follow Checkpoints For Best Experience.");
        yield return new WaitForSeconds(messageDelay);

        // 2️⃣ Message 2
        ShowMessage("Find a Safe Zone for Royal Bath.");
    }

    // ✅ Helper function to show and auto-hide messages
    private void ShowMessage(string message)
    {
        StopAllCoroutines(); // prevent overlapping
        StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        ghatInstructionScreen.SetActive(true);
        instructionText.text = message;

        yield return new WaitForSeconds(popupVisibleTime);

        ghatInstructionScreen.SetActive(false);
    }

    // ✅ External calls from CharacterAnimationController
    public void OnSafeZoneComplete()
    {
        ShowMessage("Great! Now find the next checkpoint and remove your clothes.");
    }

    public void OnClothRemoveComplete()
    {
        ShowMessage("Good job! Now find the next checkpoint and offer flowers.");
    }

    public void OnFlowerArpanComplete()
    {
        ShowMessage("Excellent! You completed all rituals successfully!");
    }
}
