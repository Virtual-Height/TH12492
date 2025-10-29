using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject welcomeScreen;
    public GameObject helpButton;
    public GameObject submitButton;
    public GameObject helpPannel;

    public Text messageText;
    public GameObject popupPannel;

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
            if (selectedAI.isLost)
            {
                selectedAI.FollowPlayer();
                PathVisualizer.Instance.SpawnArrowsAtEvenIntervals(lostAndFoundPos);
            }
            else if (selectedAI.medEmergency)
            {
                HelpPannel.instance.SetupOptions(0);
                helpPannel.SetActive(true);
            }
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