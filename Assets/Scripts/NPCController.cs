
using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        SetupClothAndHair();
        GoToRandomPoint();
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
            if(hit.collider.gameObject != gameObject)
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y;
                transform.position = pos;
            }
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
        agent.SetDestination(FindReachablePoint());

        animator.SetBool("isWalk", false);
        agent.speed = 0;

        yield return new WaitForSeconds(Random.Range(3f, 7f));

        animator.SetBool("isWalk", true);
        agent.speed = speed;
    }
}
