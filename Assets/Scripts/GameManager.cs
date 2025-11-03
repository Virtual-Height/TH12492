using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using StarterAssets;
using System.Collections;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isVR;

    public GameObject welcomeScreen;
    public GameObject helpButton;
    public GameObject submitButton;
    public GameObject helpPannel;
    public GameObject firePannel;

    public Text messageText;
    public GameObject popupPannel;

    [HideInInspector]
    public ThirdPersonController activePlayer;
    [HideInInspector]
    public NPCController selectedAI;
    [HideInInspector]
    public GameObject selectedFire;

    public Transform lostAndFoundPos;

    public bool isInFire;
    public bool smallFire;
    public bool bigFire;

    public int points;
    public Text pointsText;

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
        points = 0; //Set initial point
        pointsText.text = "Points : " + points.ToString();
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
            else if(selectedAI.isRobbed)
            {
                HelpPannel.instance.SetupOptions(1);
                helpPannel.SetActive(true);
            }
        }
        else if (isInFire)
        {
            HelpPannel.instance.SetupOptions(2);
            helpPannel.SetActive(true);
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

    public void FireOptionButtonClick(bool isSmallFire)
    {
        if(smallFire && isSmallFire)
        {
            if (!isVR)
            {
                StartCoroutine(PlayerFireAnimation());
            }
            else
            {
                //Spawn VR FireKit
            }
        }
        else if(bigFire && !isSmallFire)
        {
            messageText.text = "Correct";
            popupPannel.SetActive(true);
            AddPoint(50);
        }
        else
        {
            messageText.text = "Incorrect";
            popupPannel.SetActive(true);
        }

        if(selectedFire != null)
        {
            Destroy(selectedFire.transform.parent.gameObject, 5f);
        }

        smallFire = false;
        bigFire = false;
    }

    IEnumerator PlayerFireAnimation()
    {
        var rotation = Quaternion.LookRotation(selectedFire.transform.position - activePlayer.transform.position);
        activePlayer.transform.rotation = rotation;
        activePlayer.canMove = false;
        activePlayer.fireKit.SetActive(true);
        activePlayer.GetComponent<Animator>().SetBool("Watering", true);
        yield return new WaitForSeconds(5);
        activePlayer.GetComponent<Animator>().SetBool("Watering", false);
        activePlayer.canMove = true;
        activePlayer.fireKit.SetActive(false);

        messageText.text = "Correct";
        popupPannel.SetActive(true);
        AddPoint(50);
    }

    public void AddPoint(int point)
    {
        points += point;
        pointsText.text = "Points : " + points.ToString();
    }
}