using System.Collections;
using UnityEngine;
using StarterAssets;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;


public class CharacterUjjainController : MonoBehaviour
{
    Animator animator;
    ThirdPersonController thirdPersonController;
    bool isAnimating;

    public bool isCollectGarbage;

    public RigBuilder rigBuilder;
    public TwoBoneIKConstraint ik;

    bool isRinging = false;
    Transform currentGarbage;

    [Header("Garbage Settings")]
    public float stopDistance = 0f;
    public float moveSpeed = 1f;
    public Transform handPoint;

    [Header("UI Settings")]
    public Text pointsText;
    public Text feedbackText;

    private int totalPoints = 0;
    private string currentGarbageType = ""; // e.g. "WetWaste", "DryWaste", etc.
    private Transform carriedGarbage;       

    private CharacterController controller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        isCollectGarbage = true;
        UpdatePointsUI();
        if (feedbackText != null) feedbackText.text = "";
    }

    private void Update()
    {
        if (isRinging)
            ik.weight = Mathf.MoveTowards(ik.weight, 1f, Time.deltaTime * 1.5f);
        else
            ik.weight = Mathf.MoveTowards(ik.weight, 0f, Time.deltaTime * 1.5f);

        if (currentGarbage != null && !isAnimating)
            MoveToGarbage();


       /* if (!isAnimating && carriedGarbage == null)
            thirdPersonController.canMove = true;*/
    }


    public void TriggerGarbageCollect(Transform garbageTransform)
    {

        /*  if (!isCollectGarbage || isAnimating || carriedGarbage != null)
          {
              ShowFeedback("🚫 Drop your current garbage first!", Color.yellow);
              return;
          }*/

        Debug.Log("TriggerGarbageCollect is call ...");

        if (!isCollectGarbage || isAnimating)
        {
            thirdPersonController.canMove = true;   // ✅ Ensure movement never gets stuck
            isAnimating = false;
            Debug.Log("TriggerGarbageCollect is call ...1");
           
        }

        if (carriedGarbage != null)
        {
            // If garbageType is empty, the player actually dropped it, so clear it
            if (string.IsNullOrEmpty(currentGarbageType))
            {
                ForceClearGarbage();
                Debug.Log("TriggerGarbageCollect is call ...2");
            }
            else
            {
                ShowFeedback("🚫 Drop your current garbage first!", Color.yellow);
                return;
            }
        }

        currentGarbage = garbageTransform;
        thirdPersonController.canMove = false;

    }

    public void ForceClearGarbage()
    {
        if (carriedGarbage != null)
        {
            Destroy(carriedGarbage.gameObject);
            carriedGarbage = null;
        }

        currentGarbageType = "";
    }

    private void MoveToGarbage()
    {   
        Debug.Log("MoveToGarbage is call...");

        if (currentGarbage == null)
        {
            Debug.Log("MoveToGarbage is call...xyxytxtxtxt");
            thirdPersonController.canMove = true;
            isAnimating = false;
            return;
           
        }

        Vector3 targetPos = currentGarbage.position;
        Vector3 direction = (targetPos - transform.position);
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            direction.Normalize();

            controller.Move(direction * moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
            animator.SetFloat("Speed", 2f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            StartCoroutine(CollectGarbage(currentGarbage));
            currentGarbage = null;
        }  

    }

    IEnumerator CollectGarbage(Transform garbageTransform)
    {
        isAnimating = true;
        isRinging = true;
        rigBuilder.enabled = true;

        thirdPersonController.canMove = false;
        animator.SetTrigger("PickupGarbage");

        if (garbageTransform.CompareTag("WetWaste"))
            currentGarbageType = "WetWaste";
        else if (garbageTransform.CompareTag("DryWaste"))
            currentGarbageType = "DryWaste";
        else if (garbageTransform.CompareTag("HazardousWaste"))
            currentGarbageType = "HazardousWaste";
        else if (garbageTransform.CompareTag("SanitaryWaste"))
            currentGarbageType = "SanitaryWaste";
        else
            currentGarbageType = "";

        yield return new WaitForSeconds(1.2f);

        if (garbageTransform != null)
        {
        
            garbageTransform.SetParent(handPoint);
            garbageTransform.localPosition = Vector3.zero;
            garbageTransform.localRotation = Quaternion.identity;

            carriedGarbage = garbageTransform; 
        }

        yield return new WaitForSeconds(0.8f);

        isRinging = false;
        thirdPersonController.canMove = true;
        rigBuilder.enabled = false;
        isAnimating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(currentGarbageType) || carriedGarbage == null)
            return; 

        string binTag = other.tag;
        bool isCorrect = false;

        
        if (binTag == "GreenBin" && currentGarbageType == "WetWaste") isCorrect = true;
        else if (binTag == "BlueBin" && currentGarbageType == "DryWaste") isCorrect = true;
        else if (binTag == "RedBlackBin" && currentGarbageType == "HazardousWaste") isCorrect = true;
        else if (binTag == "RedBin" && currentGarbageType == "SanitaryWaste") isCorrect = true;

        if (isCorrect)
        {
            AddPoints(10);
            ShowFeedback("+10 Points!", Color.green);
            Debug.Log("✅ Correct bin: " + binTag);
        }
        else if (binTag.Contains("Bin"))
        {
            ShowFeedback("❌ Wrong Bin!", Color.red);
            Debug.Log("❌ Wrong bin for " + currentGarbageType);
        }
        else
        {
            return; 
        }
       
        if (carriedGarbage != null)
        {
            UIManager.Instance.currentGarbageList.Remove(carriedGarbage);

            if (UIManager.Instance.currentGarbageList.Count == 0)
            {
                Debug.Log("🎉 All garbage collected! Task Completed!");
                UIManager.Instance.OngrabgeCompleted();
            }

            Destroy(carriedGarbage.gameObject);
            carriedGarbage = null;
        }

        ClearHeldGarbage();
    }

    private void AddPoints(int amount)
    {
        totalPoints += amount;
        UpdatePointsUI();
    }

    private void UpdatePointsUI()
    {
        if (pointsText != null)
            pointsText.text = "Points: " + totalPoints;
    }

    private void ClearHeldGarbage()
    {
        currentGarbageType = "";
    }

    private void ShowFeedback(string message, Color color)
    {
        if (feedbackText == null) return;

        feedbackText.text = message;
        feedbackText.color = color;
        StopAllCoroutines();
        StartCoroutine(ClearFeedback());
    }

    IEnumerator ClearFeedback()
    {
        yield return new WaitForSeconds(2f);
        if (feedbackText != null)
            feedbackText.text = "";
    }
   
}