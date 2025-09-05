using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;

public class PathVisualizer : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject arrowPerent;
    public float arrowSpacing = 2f;
    public GameObject mapObject;

    public InputActionReference mapButton;

    private void Awake()
    {
        mapButton.action.performed += OpenMap;
    }

    private void OpenMap(InputAction.CallbackContext obj)
    {
        mapObject.SetActive(true);
    }

    public void SpawnArrowsAtEvenIntervals(Transform target)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
        {
            ClearArrows();
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 10f);
            }
            Vector3 previousPoint = path.corners[0];

            for (int i = 1; i < path.corners.Length; i++)
            {
                Vector3 currentPoint = path.corners[i];
                float segmentLength = Vector3.Distance(previousPoint, currentPoint);
                int arrowsInSegment = Mathf.FloorToInt(segmentLength / arrowSpacing);
                for (int j = 1; j <= arrowsInSegment; j++)
                {
                    float distanceAlongSegment = j * arrowSpacing;
                    Vector3 arrowPosition = previousPoint + (currentPoint - previousPoint).normalized * distanceAlongSegment;
                    Debug.DrawLine(previousPoint, arrowPosition, Color.green, 10f);
                    GameObject arrow = Instantiate(arrowPrefab, arrowPosition, Quaternion.identity);
                    Vector3 directionToNextPoint = currentPoint - arrowPosition;

                    Debug.DrawRay(arrowPosition, directionToNextPoint, Color.blue, 10f);
                    Quaternion targetRotation = Quaternion.LookRotation(directionToNextPoint);
                    arrow.transform.rotation = targetRotation;
                    arrow.transform.SetParent(arrowPerent.transform);
                }
                previousPoint = currentPoint;
            }
        }
        mapObject.SetActive(false);
    }

    public void ClearArrows()
    {
        Debug.Log("Arrows Cleared...!");

        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject arrow in arrows)
        {
            Destroy(arrow);
        }
    }
}