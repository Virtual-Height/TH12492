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

    public GameObject popup;
    public TextMeshPro popupText;
    public bool isLost;
    public bool medEmergency;
    public bool isRobbed;
    public bool followPlayer;

    public List<Transform> helpingPlayers = new List<Transform>();

    public string npcType;


    void Start()
    {
        popup.SetActive(false);
        popupText.gameObject.SetActive(false);
        SetupClothAndHair();
        TriggerBehaviorAtStart();
       // Lost();
          //Robbed();
       //MedicalEmergency();    
        // GoToRandomPoint();
    }

    private void TriggerBehaviorAtStart()
    {
        float chance = Random.Range(0f, 1f);

        if (chance < 0.1f)
        {
            CallRandomEvent();
        }
        else
        {
            Debug.Log("AI continues with normal behavior.");
        }
    }

    private void CallRandomEvent()
    {
        int randomEvent = Random.Range(0, 3);

        switch (randomEvent)
        {
            case 0:
                Lost();
                break;
            case 1:
                MedicalEmergency();
                break;
            case 2:
                Robbed();
                break;
        }
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
            if (hit.collider.gameObject != this.gameObject)
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
        if (followPlayer || GameManager.instance.selectedAI != null)
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
        if (!isLost && !medEmergency && !isRobbed)
        {
            agent.SetDestination(FindReachablePoint());

            animator.SetBool("isWalk", false);
            agent.speed = 0;

            yield return new WaitForSeconds(Random.Range(3f, 7f));

            animator.SetBool("isWalk", true);
            agent.speed = speed;
        }

        yield return new WaitForSeconds(Random.Range(10f, 15f));
        StartCoroutine(GoToRandomPoint());
    }

    public void FollowPlayer()
    {
        followPlayer = true;
    }

    void Lost()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        isLost = true;
        popup.SetActive(true);
        popupText.gameObject.SetActive(true);
        popupText.text = "I am lost...Help!";
        UIManager.Instance.activeHelp = "LostItem";
    }

    void MedicalEmergency()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        medEmergency = true;
        popup.SetActive(true);
        popupText.gameObject.SetActive(true);
        popupText.text = "I have medical condition...Help!";
        animator.SetTrigger("Medical");

        UIManager.Instance.activeHelp = "Medical";
    }

    void Robbed()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        isRobbed = true;
        popup.SetActive(true);
        popupText.gameObject.SetActive(true);
        popupText.text = "I lost my bag...Help!";
        animator.SetTrigger("Robbed");
        UIManager.Instance.activeHelp = "Police";
    }

    public void ReportToLostAndFound()
    {
        followPlayer = false;
        popup.SetActive(false);
        popupText.gameObject.SetActive(false);
        Destroy(this.gameObject, 10f); 

    }

    public void AddPlayer(Transform player)
    {
        if (!helpingPlayers.Contains(player))
        {
            helpingPlayers.Add(player);
            Debug.Log("Player added to NPC list: " + player.name);
        }
    }
  

}