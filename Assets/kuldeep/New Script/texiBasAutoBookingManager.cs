using UnityEngine;

public class texiBasAutoBookingManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.vechicleBookingScreen.SetActive(true);

        }
    }
}
