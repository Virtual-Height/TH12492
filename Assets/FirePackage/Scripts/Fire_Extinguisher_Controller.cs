using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Fire_Extinguisher_Controller : MonoBehaviour
{
    public GameObject particle;
    bool canFire;
    //public InputActionReference fire;

    //private void Awake()
    //{
    //    fire.action.started += Fire;
    //    fire.action.canceled += StopFire;
    //}

    public void TrunOn()
    {
        canFire = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public void TrunOff()
    {
        canFire = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<BoxCollider>().isTrigger = false;
        particle.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            {
                particle.SetActive(true);
            }
        }
        else
        {
            {
                particle.SetActive(false);
            }
        }
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (canFire)
        {
            particle.SetActive(true);
        }
    }

    private void StopFire(InputAction.CallbackContext context)
    {
        if (canFire)
        {
            particle.SetActive(false);
        }
    }
}
