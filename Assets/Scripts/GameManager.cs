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

    public GameObject lostAndFoundPosPoint;

    [Header("Firefighter Settings")]
    public GameObject[] fireFighterPrefab;
    public GameObject[] fireTruckPrefab;
    public float fireSpawnRadius = 3f;

    public float fireCleanupDelay = 10f;

    private GameObject spawnedFireTruck;
    private GameObject spawnedFireFighter;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (activePlayer == null)
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
        if (selectedAI != null && activePlayer != null)
        {
            if (selectedAI.isLost)
            {
                selectedAI.FollowPlayer();
                PathVisualizer.Instance.SpawnArrowsAtEvenIntervals(lostAndFoundPos);

                //active check point  
                lostAndFoundPosPoint.SetActive(true);
            }
            else if (selectedAI.medEmergency)
            {
                HelpPannel.instance.SetupOptions(0);
                helpPannel.SetActive(true);

            }
            else if (selectedAI.isRobbed)
            {
                HelpPannel.instance.SetupOptions(1);
                helpPannel.SetActive(true);
            }
        } 

        else if (isInFire)
        {
            UIManager.Instance.activeHelp = "Fire";
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

            //deactivate checkpoint
            lostAndFoundPosPoint.SetActive(false);
            selectedAI = null;
        }
        submitButton.SetActive(false);
        popupPannel.SetActive(true);
    }

    public void FireOptionButtonClick(bool isSmallFire)
    {

        UIManager.Instance.activeHelp = "Fire";

        if (smallFire && isSmallFire)
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
        else if (bigFire && !isSmallFire)
        {

            SpawnFireResponseTeam();

            // Spawn Firefighter
            messageText.text = "Correct";
            popupPannel.SetActive(true);
            AddPoint(50);
        }
        else
        {
            messageText.text = "Incorrect";
            popupPannel.SetActive(true);
        }

        if (selectedFire != null)
        {
            Destroy(selectedFire.transform.parent.gameObject, 5f);
        }

        if (spawnedFireTruck != null)
        {
            Destroy(spawnedFireTruck, 5f);
        }

        if (spawnedFireFighter != null)
        {
            Destroy(spawnedFireFighter, 5f);
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

    private void SpawnFireResponseTeam()
    {
        Transform player = GameManager.instance.activePlayer?.transform;
        if (player == null) return;

        // Spawn firefighter
        if (fireFighterPrefab.Length > 0)
        {
            Vector3 personPos = GetRandomPositionNear(player.position, fireSpawnRadius);
            spawnedFireFighter = Instantiate(
                fireFighterPrefab[Random.Range(0, fireFighterPrefab.Length)],
                personPos,
                Quaternion.identity
            );
            spawnedFireFighter.transform.LookAt(player);
        }

        // Spawn fire truck
        if (fireTruckPrefab.Length > 0)
        {
            Vector3 vehiclePos = GetRandomPositionNear(player.position, fireSpawnRadius + 2f);
            spawnedFireTruck = Instantiate(
                fireTruckPrefab[Random.Range(0, fireTruckPrefab.Length)],
                vehiclePos,
                Quaternion.identity
            );
            spawnedFireTruck.transform.LookAt(player);
        }
    }


    private Vector3 GetRandomPositionNear(Vector3 center, float radius)
    {
        return center + new Vector3(
            Random.Range(-radius, radius),
            0,
            Random.Range(-radius, radius)
        );
    }


}