using JetBrains.Annotations;
using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.helpButton.SetActive(true);
            GameManager.instance.isInFire = true;
            GameManager.instance.selectedFire = this.gameObject;

            if (this.CompareTag("SmallFire"))
            {
                GameManager.instance.smallFire = true;
            }
            else if (this.CompareTag("BigFire"))
            {
                GameManager.instance.bigFire = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.helpButton.SetActive(false);
            GameManager.instance.isInFire = false;
            GameManager.instance.smallFire = false;
            GameManager.instance.bigFire = false;
            GameManager.instance.selectedFire = null;
        }
    }
}
