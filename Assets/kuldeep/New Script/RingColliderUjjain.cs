using UnityEngine;

public class RingColliderUjjain : MonoBehaviour
{
    public Transform ikTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterUjjainController>().isCollectGarbage = true;
            other.GetComponent<CharacterUjjainController>().ik.data.target = ikTarget;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterUjjainController>().isCollectGarbage = false;
            other.GetComponent<CharacterUjjainController>().ik.data.target = null;
            
            
        }
    }
}
