using UnityEngine;
using System.Threading.Tasks;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject welcomeScreen;
    public GameObject helpButton;

    public ThirdPersonController activePlayer;
    public NPCController selectedAI;
    public Transform lostAndFoundPos;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if(activePlayer == null)
        {
            activePlayer = FindFirstObjectByType<ThirdPersonController>();
        }
    }

    async void Start()
    {
        welcomeScreen.SetActive(true);
        await Task.Delay(7000);
        welcomeScreen.SetActive(false);
    }

    public void HelpButtonClick()
    {
        if(selectedAI != null && activePlayer != null)
        {
            selectedAI.FollowPlayer();
            PathVisualizer.Instance.SpawnArrowsAtEvenIntervals(lostAndFoundPos);
        }
        helpButton.SetActive(false);
    }
}