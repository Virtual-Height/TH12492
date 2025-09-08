using UnityEngine;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public GameObject welcomeScreen;

    async void Start()
    {
        welcomeScreen.SetActive(true);
        await Task.Delay(7000);
        welcomeScreen.SetActive(false);
    }
}
