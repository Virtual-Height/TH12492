
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float walkRadius = 50;
    public float speed;

    public Animator animator;

    float dist;

    public Renderer topRenderer;
    public Renderer bottomRenderer;

    public GameObject[] hairList;
    public Material[] topMatList;
    public Material[] bottomMatList;

    public TextMeshPro popupText;
    public bool isLost;
    public bool medEmergency;
    public bool followPlayer;


    void Start()
    {
        SetupClothAndHair();
        GoToRandomPoint();
        //Lost();
        MedicalEmergency();
    }

    private void Update()
    {
        dist = agent.remainingDistance;
        if (dist < .5f)
        {
            StartCoroutine(GoToRandomPoint());
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out hit, 50f))
        {
            if(hit.collider.gameObject != this.gameObject)
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y;
                transform.position = pos;
            }
        }

        if (followPlayer)
        {
            agent.SetDestination(GameManager.instance.activePlayer.transform.position);
            agent.speed = 4.5f;

            if (agent.velocity.magnitude > 0)
            {
                animator.SetBool("isWalk", true);
            }
            else
            {
                animator.SetBool("isWalk", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(followPlayer || GameManager.instance.selectedAI != null)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            GameManager.instance.helpButton.SetActive(true);
            GameManager.instance.selectedAI = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (followPlayer)
        {
            Debug.Log("auysdbfloafd");
            return;
        }
        if (other.CompareTag("Player"))
        {
            GameManager.instance.helpButton.SetActive(false);
            GameManager.instance.selectedAI = null;
        }
    }

    void SetupClothAndHair()
    {
        int hairIndex = Random.Range(0, hairList.Length);
        int topIndex = Random.Range(0, topMatList.Length);
        int bottomIndex = Random.Range(0, bottomMatList.Length);

        hairList[hairIndex].SetActive(true);

        topRenderer.material = topMatList[topIndex];
        bottomRenderer.material = bottomMatList[bottomIndex];
    }

    Vector3 FindReachablePoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position + new Vector3(0, 5, 0);

        return finalPosition;
    }

    IEnumerator GoToRandomPoint()
    {
        if (!isLost && !medEmergency)
        {
            agent.SetDestination(FindReachablePoint());

            animator.SetBool("isWalk", false);
            agent.speed = 0;

            yield return new WaitForSeconds(Random.Range(3f, 7f));

            animator.SetBool("isWalk", true);
            agent.speed = speed;
        }
    }

    public void FollowPlayer()
    {
        followPlayer = true;
    }

    void Lost()
    {
        isLost = true;
        popupText.gameObject.SetActive(true);
    }

    void MedicalEmergency()
    {
        medEmergency = true;
        popupText.gameObject.SetActive(true);
        animator.SetTrigger("Medical");
    }

    public void ReportToLostAndFound()
    {
        followPlayer = false;
        popupText.gameObject.SetActive(false);

        Destroy(this.gameObject, 10f);
    }
}