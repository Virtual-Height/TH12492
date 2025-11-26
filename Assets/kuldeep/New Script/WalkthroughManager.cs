using UnityEngine;

public class WalkthroughManager : MonoBehaviour
{
    public static WalkthroughManager Instance;

    [Header("Rail Settings")]
    public GameObject railObject;
    public float railMoveDistance = 5f;
    public float railMoveSpeed = 2f;
    public Vector3 railDirection = Vector3.forward;
    public Transform railTeleportTarget;

    [Header("Bus Settings")]
    public GameObject busObject;
    public float busMoveDistance;
    public float busMoveSpeed;
    public Vector3 busDirection;
    public Transform busTeleportTarget;

    [Header("Flight Settings")]
    public GameObject flightObject;
    public float flightMoveDistance = 5f;
    public float flightMoveSpeed = 2f;
    public Vector3 flightDirection = Vector3.forward;
    public Transform flightTeleportTarget;

    private bool startWalkthrough = false;
    private bool walkthroughCompleted = false;

    private Transform activeTransform;
    private float activeSpeed;
    private Vector3 targetPos;

    private Transform playerToMove;
    private Transform currentTeleportTarget;

    [Header("Camera")]
    public GameObject railwayCamera;
    public GameObject flightCamera;
    public GameObject basCamera;

    public string lastTeleportPoint = "";

    public bool isfadeInOut;

    [Header("Fade Settings")]
    public MeshRenderer railFadeMesh;
    public MeshRenderer busFadeMesh;
    public MeshRenderer flightFadeMesh;

    private MeshRenderer activeFadeMesh;
    private float fadeValue = 0f;
    private bool fadeStarted = false;

    [Header("Fade Time")]
    public float railFadeSpeed = 0.3f;
    public float busFadeSpeed = 0.3f;
    public float flightFadeSpeed = 0.3f;

    private float activeFadeSpeed = 0.3f;
    private float activeMoveDistance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //StartWalkthrough("Rail");
    }

    public void StartWalkthrough(string transportType)
    {
        Debug.Log("Walkthrough Start For : " + transportType);

        startWalkthrough = true;
        walkthroughCompleted = false;

        // Disable all first
        railObject.SetActive(false);
        busObject.SetActive(false);
        flightObject.SetActive(false);

        railObject.SetActive(false);
        busObject.SetActive(false);
        flightObject.SetActive(false);

        switch (transportType)
        {
            case "Rail":
                railObject.SetActive(true);
                activeTransform = railObject.transform;

                targetPos = activeTransform.position +
                            railDirection.normalized * railMoveDistance;

                activeSpeed = railMoveSpeed;
                activeMoveDistance = railMoveDistance;

                currentTeleportTarget = railTeleportTarget;
                lastTeleportPoint = "Rail";

                activeFadeMesh = railFadeMesh;
                activeFadeSpeed = railFadeSpeed;
                break;

            case "Bus":
                busObject.SetActive(true);
                activeTransform = busObject.transform;

                targetPos = activeTransform.position +
                            busObject.transform.forward.normalized * -busMoveDistance;

                activeSpeed = busMoveSpeed;
                activeMoveDistance = busMoveDistance;

                currentTeleportTarget = busTeleportTarget;
                lastTeleportPoint = "Bus";

                activeFadeMesh = busFadeMesh;
                activeFadeSpeed = busFadeSpeed;
                break;

            case "Flight":
                flightObject.SetActive(true);
                activeTransform = flightObject.transform;

                targetPos = activeTransform.position +
                            flightDirection.normalized * flightMoveDistance;

                activeSpeed = flightMoveSpeed;
                activeMoveDistance = flightMoveDistance;

                currentTeleportTarget = flightTeleportTarget;
                lastTeleportPoint = "Flight";

                activeFadeMesh = flightFadeMesh;
                activeFadeSpeed = flightFadeSpeed;
                break;
        }

    }

    private void Update()
    {
        if (!startWalkthrough || activeTransform == null) return;

        // ----------- MOVEMENT -----------
        activeTransform.position = Vector3.MoveTowards(
            activeTransform.position,
            targetPos,
            activeSpeed * Time.deltaTime
        );

        float totalDistance = Vector3.Distance(activeTransform.position, targetPos + (activeTransform.position - targetPos).normalized * activeSpeed);
        float remainingDistance = Vector3.Distance(activeTransform.position, targetPos);
       // float movementProgress = 1f - (remainingDistance / railMoveDistance);  // 0 → 1 progress
        float movementProgress = 1f - (remainingDistance / activeMoveDistance);

        // ⭐ START FADE AT 90% OF MOVEMENT
        if (!fadeStarted && movementProgress >= 0.75f)
        {
            fadeStarted = true;
            fadeValue = 0f;
        }

        // ⭐ MOVEMENT FINISHED (100%)
        if (!walkthroughCompleted && remainingDistance < 0.05f)
        {
            walkthroughCompleted = true;
            startWalkthrough = false;

            Debug.Log("Movement fully completed! Teleport will run now.");

            OnWalkthroughCompleted(); // teleport NOW (not after fade)
        }

        // ----------- FADE EFFECT -----------
        if (fadeStarted && activeFadeMesh != null)
        {
            //fadeValue += Time.deltaTime * 0.3f;

            fadeValue += Time.deltaTime * activeFadeSpeed;

            activeFadeMesh.material.SetFloat("_FallValue", fadeValue);

            if (fadeValue >= 1f)
            {
                fadeStarted = false;
                Debug.Log("Fade completed!");
            }
        }
    }

    private void OnWalkthroughCompleted()
    {
        Debug.Log("Walkthrough Completed Successfully!");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player with tag 'Player' not found!");
            return;
        }

        playerToMove = playerObj.transform;

        if (currentTeleportTarget == null)
        {
            Debug.LogError("No teleport target assigned for current transport!");
            return;
        }

        CharacterController cc = playerToMove.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // ⭐ TELEPORT PLAYER TO THE SELECTED TARGET
        playerToMove.position = currentTeleportTarget.position;
        playerToMove.rotation = currentTeleportTarget.rotation;

        if (cc != null) cc.enabled = true;

        Debug.Log("Player Teleported to: " + currentTeleportTarget.name);

        railwayCamera.SetActive(false);
        basCamera.SetActive(false);
        flightCamera.SetActive(false);   
    }
}