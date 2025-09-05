using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public Animator m_animator;

    private void Start()
    {
        this.m_animator.SetBool("Open", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        this.m_animator.SetBool("Open", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }
        this.m_animator.SetBool("Open", false);
    }
}
