using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float reachDistance = 0.1f;

    private int currentIndex = 0;
    private bool movementComplete = false;

    [Header("Teleport Settings")]
    public Transform teleportTarget;

    [Header("Fade Settings")]
    public MeshRenderer fadeMesh;
    private float fadeValue = 0f;
    private float fadeSpeed = 0.5f;
    private bool fadeInStarted = false;
    private bool fadeOutStarted = false;

    // ⭐ Fade should start 10 seconds before teleport
    public float fadeStartBeforeTeleport = 10f;


    private void Update()
    {
        if (movementComplete) return;
        if (waypoints.Length == 0) return;

        // ⭐ KEEP CURRENT Y (no up/down movement)
        Vector3 targetPos = new Vector3(
            waypoints[currentIndex].position.x,
            transform.position.y,
            waypoints[currentIndex].position.z
        );

        // Move towards current waypoint
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // -------- BEFORE TELEPORT: CHECK TIME REMAINING --------
        float distanceToLastPoint = Vector3.Distance(transform.position, waypoints[waypoints.Length - 1].position);
        float timeRemaining = distanceToLastPoint / moveSpeed;

        if (!fadeInStarted && timeRemaining <= fadeStartBeforeTeleport)
        {
            fadeInStarted = true;
            fadeValue = 0;
            Debug.Log("⭐ Fade-In started (10 seconds before teleport)");
        }

        // -------- WAYPOINT REACHED --------
        if (Vector3.Distance(transform.position, targetPos) < reachDistance)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                movementComplete = true;
                Debug.Log("Movement Complete!");
            }
        }

        // -------- FADING SYSTEM --------
        HandleFadeEffect();
    }
    private void HandleFadeEffect()
    {
        // ⭐ FADE IN → 0 to 1
        if (fadeInStarted && !fadeOutStarted)
        {
            fadeValue += fadeSpeed * Time.deltaTime;
            fadeMesh.material.SetFloat("_FallValue", fadeValue);

            if (fadeValue >= 1f)
            {
                Debug.Log("⭐ Fade-In Completed — Teleporting now");
                fadeOutStarted = true;
                fadeInStarted = false;

                UIManager.Instance.gamePlayScreen.SetActive(true);
                TeleportPlayer();
               
            }
        }

        // ⭐ FADE OUT → 1 to 0
        else if (fadeOutStarted)
        {
            fadeValue -= fadeSpeed * Time.deltaTime;
            fadeMesh.material.SetFloat("_FallValue", fadeValue);

            if (fadeValue <= 0f)
            { 
                fadeOutStarted = false;
                Debug.Log("⭐ Fade-Out Complete");
               
            }
        }

    }

    private void TeleportPlayer()
    {
        if (!teleportTarget)
        {
            Debug.LogError("❌ No teleport target assigned!");
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (!playerObj)
        {
            Debug.LogError("❌ Player not found!");
            return;
        }

        Transform player = playerObj.transform;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.position = teleportTarget.position;

        if (cc) cc.enabled = true;

        Debug.Log("🚀 Player Teleported!");

        // Disable object after fade-out ends
        Invoke(nameof(DisableObject), 0.1f);

        
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }

}