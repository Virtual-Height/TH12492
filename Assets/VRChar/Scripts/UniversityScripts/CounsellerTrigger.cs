using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CounsellerTrigger : MonoBehaviour
{
    public Counseller counseller;
    public CounsellerTrigger[] counsellerTriggers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.counseller.StartDialogue();
            foreach (CounsellerTrigger a in counsellerTriggers)
            {
                a.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
