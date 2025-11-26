using System.Collections.Generic;
using UnityEngine;

public class GarbageZone : MonoBehaviour
{
    [Header("Assign Garbage Objects Manually")]
    public List<Transform> garbageObjects = new List<Transform>();

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Copy garbage list to UIManager
            UIManager.Instance.currentGarbageList = new List<Transform>(garbageObjects);

            Debug.Log("Player entered GarbageZone → Loaded Garbage List: "
                      + UIManager.Instance.currentGarbageList.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.currentGarbageList.Clear();
        }
    }

}