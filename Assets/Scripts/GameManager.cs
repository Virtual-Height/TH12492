using UnityEngine;
using System.Threading.Tasks;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject welcomeScreen;
    public GameObject helpButton;
    public GameObject submitButton;
    public GameObject popupPannel;

    [HideInInspector]
    public ThirdPersonController activePlayer;
    [HideInInspector]
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

    public void SubmitButtonClick()
    {
        if (selectedAI != null && activePlayer != null)
        {
            selectedAI.ReportToLostAndFound();
            PathVisualizer.Instance.ClearArrows();
            selectedAI = null;
        }
        submitButton.SetActive(false);
        popupPannel.SetActive(true);
    }
}