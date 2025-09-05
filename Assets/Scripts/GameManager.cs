using UnityEngine;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public GameObject welcomeScreen;

    async void Start()
    {
        await Task.Delay(10000);
        welcomeScreen.SetActive(false);
    }
}
