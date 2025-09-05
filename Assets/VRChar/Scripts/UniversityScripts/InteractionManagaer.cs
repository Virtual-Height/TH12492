using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManagaer : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("OuterDoor"))
        {
            animator.SetTrigger("OuterOpen");
        }
        if (collision.gameObject.CompareTag("InnerDoor"))
        {
            animator.SetTrigger("InnerOpen");
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("OuterDoor"))
        {
            animator.SetTrigger("OuterClose");
        }
        if (collision.gameObject.CompareTag("InnerDoor"))
        {
            animator.SetTrigger("InnerClose");
        }
    }
}