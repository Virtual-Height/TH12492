using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Main Transport Buttons")]
    public Button flightButton;
    public Button railButton;
    public Button busButton;
    public Button nextButton;

    [Header("Popup Panels")]
    public GameObject flightPopup;
    public GameObject railPopup;
    public GameObject busPopup;
    public GameObject vechicleBookingScreen;
    public GameObject gamePlayScreen;
    public GameObject taskPopupScreen;
    public GameObject activityScreen;

    [Header("Go Activity")]
    public Transform QuizOneTeleportTarget;
    public Transform QuizTwoTeleportTarget;
    public Transform QuizThreeTeleportTarget;
    public Transform QuizFourTeleportTarget;

    [Space(20)]
    public Transform[] garbageTeleportTarget;

    [Space(20)]
    public Transform[] ghatShahiSnanTeleportTarget;

    private Transform playerToMove;

    public Transform fireEmergencyHelpTeleportTarget;
    private Transform medicalEmergencyHelpTeleportTarget;
    private Transform lostItemsPeopleSupportTeleportTarget;
    private Transform peopleSupportTeleportTarget;


    [Header("Quiz Image Marks")]
    public Image quizOneButton;
    public Image quizTwoButton;
    public Image quizThreeButton;
    public Image quizFourButton;
    public string activeQuiz = "";

    [Header("Help Image Marks")]
    public Image medicalHelpButton;
    public Image fireHelpButton;
    public Image policeHelpButton;
    public Image lostItemHelpButton;
    public string activeHelp = "";

    [Header("Ghat Activity Button")]
    public Image ghatShahiSnanButton;
    public Image ghatClothChangeRitualButton;
    public Image ghatDivineAartiRitualFlowerOfferingRitualButton;
    public bool ghatActivityCompleted = false;

    private string selectedTransport = null;
    private string selectedCab = null;

    [Header("Button Colors")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    [Header("Garbage Message")]
    public GameObject garbageMessage;
    public bool garbageMessageShown = false;

    [Header("Vehicle Buttons")]
    public Button autoButton;
    public Button taxiButton;
    public Button cityBusButton;
    public Button cabNextButton;

    [Header("AirPort")]
    public GameObject airportAutoObject;
    public GameObject airportCityBasObject;
    public GameObject airportTaxiObject;

    [Header("BusStand")]
    public GameObject busStandAutoObject;
    public GameObject busStandBasObject;
    public GameObject busStandTaxiObject;

    [Header("Railway")]
    public GameObject railwayAutoObject;
    public GameObject railwayCItyBasObject;
    public GameObject railwayTaxiObject;

    public List<NPCController> npcList = new List<NPCController>();

    [Header("Garbage System")]
    public List<Transform> currentGarbageList = new List<Transform>();
    public Image garbageIconImage; // Assign in Inspector

    [Header("Completion")]
    public GameObject congratulationsPanel;
    public int completedTaskCount = 0;
    int totalTasks = 12;   // example (4 quizzes + 4 help + garbage + ghat etc.)
    bool finalCompletedShown = false;

    [Header("Activity Button")]
    public Button quizOne;
    public Button quizTwo;
    public Button quizThree;
    public Button quizFour;
    public Button medicalHelp;
    public Button fireHelp;
    public Button policeHelp;
    public Button lostItemHelp;
    public Button ghatShahiSnan;
    public Button ghatDivineAartiRitualFlowerOfferingRitual;
    public Button ghatClothChangeRitual;
    public Button garbageIcon;

    public bool isTest=false;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        QuizManager.instance.OnQuizComplete += OnQuizFinished;

        nextButton.interactable = false;
        flightPopup.SetActive(false);
        railPopup.SetActive(false);
        busPopup.SetActive(false);

        flightButton.onClick.AddListener(() => OnTransportSelected("Flight"));
        railButton.onClick.AddListener(() => OnTransportSelected("Rail"));
        busButton.onClick.AddListener(() => OnTransportSelected("Bus"));
        nextButton.onClick.AddListener(OnNextClicked);

        autoButton.onClick.AddListener(() => OnCabSelected("Auto"));
        taxiButton.onClick.AddListener(() => OnCabSelected("Taxi"));
        cityBusButton.onClick.AddListener(() => OnCabSelected("CityBus"));

        cabNextButton.onClick.AddListener(OnCabNextClicked);

    }

    private void OnTransportSelected(string type)
    {
        selectedTransport = type;
        nextButton.interactable = true;
        Debug.Log("Selected Transport: " + selectedTransport);

        ResetButtonColors();

        switch (type)
        {
            case "Flight":
                flightButton.image.color = highlightColor;
                break;
            case "Rail":
                railButton.image.color = highlightColor;
                break;
            case "Bus":
                busButton.image.color = highlightColor;
                break;
        }
    }

    private void ResetButtonColors()
    {
        flightButton.image.color = normalColor;
        railButton.image.color = normalColor;
        busButton.image.color = normalColor;

        autoButton.image.color = normalColor;
        cityBusButton.image.color = normalColor;
        taxiButton.image.color = normalColor;

    }

    private void OnNextClicked()
    {
        if (string.IsNullOrEmpty(selectedTransport)) return;

        flightPopup.SetActive(false);
        railPopup.SetActive(false);
        busPopup.SetActive(false);

        switch (selectedTransport)
        {
            case "Flight":
                flightPopup.SetActive(true);
                break;
            case "Rail":
                railPopup.SetActive(true);
                break;
            case "Bus":
                busPopup.SetActive(true);
                break;
        }

        Debug.Log("Opened popup for: " + selectedTransport);
    }

    public void UnhideCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseActivityPannel()
    {
        activityScreen.SetActive(false);
        FindFirstObjectByType<CharacterAnimationController>().ActivityDone();
    }

    public void FlowerAarpanEvent()
    {
        FindFirstObjectByType<CharacterAnimationController>().FlowerAarpanEvent();
        Debug.Log("FlowerAarpanEvent is call...");  
        isTest=true;
    }    

    public void AratiEvent()
    {
        FindFirstObjectByType<CharacterAnimationController>().AratiEvent();
        Debug.Log("AratiEventEvent is call...");
        isTest = true;
    }

    //cab booking 

    private void OnCabSelected(string type)
    {
        selectedCab = type;
        cabNextButton.interactable = true;

        ResetButtonColors();

        switch (type)
        {
            case "Auto":
                autoButton.image.color = highlightColor;
                break;
            case "Taxi":
                taxiButton.image.color = highlightColor;
                break;
            case "CityBus":
                cityBusButton.image.color = highlightColor;
                break;
        }
    }
    private void OnCabNextClicked()
    {
        string point = WalkthroughManager.Instance.lastTeleportPoint;

        DisableAllCabObjects();

        if (point == "Rail")
        {
            ShowSelectedCabAtRailway();
        }
        else if (point == "Bus")
        {
            ShowSelectedCabAtBusStand();
        }
        else if (point == "Flight")
        {
            ShowSelectedCabAtAirport();
        }
        vechicleBookingScreen.SetActive(false);
    }
    private void ShowSelectedCabAtRailway()
    {
        if (selectedCab == "Auto") railwayAutoObject.SetActive(true);
        if (selectedCab == "CityBus") railwayCItyBasObject.SetActive(true);
        if (selectedCab == "Taxi") railwayTaxiObject.SetActive(true);
    }
    private void ShowSelectedCabAtBusStand()
    {
        if (selectedCab == "Auto") busStandAutoObject.SetActive(true);
        if (selectedCab == "CityBus") busStandBasObject.SetActive(true);
        if (selectedCab == "Taxi") busStandTaxiObject.SetActive(true);
    }
    private void ShowSelectedCabAtAirport()
    {
        if (selectedCab == "Auto") airportAutoObject.SetActive(true);
        if (selectedCab == "CityBus") airportCityBasObject.SetActive(true);
        if (selectedCab == "Taxi") airportTaxiObject.SetActive(true);
    }
    public void DisableAllCabObjects()
    {
        // Airport
        airportAutoObject.SetActive(false);
        airportCityBasObject.SetActive(false);
        airportTaxiObject.SetActive(false);

        // Bus Stand
        busStandAutoObject.SetActive(false);
        busStandBasObject.SetActive(false);
        busStandTaxiObject.SetActive(false);

        // Railway
        railwayAutoObject.SetActive(false);
        railwayCItyBasObject.SetActive(false);
        railwayTaxiObject.SetActive(false);
    }

    public void ShowGarbageMessageOnce()
    {
        if (garbageMessageShown) return;  // ❌ Already shown, do not show again

        garbageMessageShown = true;       // Mark as shown

        garbageMessage.SetActive(true);
        Invoke(nameof(HideGarbageMessage), 10f); // Auto-hide
    }
    private void HideGarbageMessage()
    {
        garbageMessage.SetActive(false);
    }
    public void ShowTaskBtnClick()
    {
        taskPopupScreen.SetActive(true);
    }
    public void ShowTaskCloseBtnClick()
    {
        taskPopupScreen.SetActive(false);
    }

    // ----------------------- TELEPORT SYSTEM -----------------------
    public void TeleportPlayer(Transform target)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerToMove = player.transform;

        CharacterController cc = playerToMove.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        if (player == null || target == null)
        {
            Debug.LogError("Player or Teleport Target Missing!");
            return;
        }

        // Teleport player
        player.transform.position = target.position;
        player.transform.rotation = target.rotation;

        if (cc != null) cc.enabled = true;

        taskPopupScreen.SetActive(false);

    }
    public void GoQuizOne()
    {
        Debug.Log("GoQuizOne is call ...");
        activeQuiz = "Quiz1";
        TeleportPlayer(QuizOneTeleportTarget);
    }
    public void GoQuizTwo()
    {
        activeQuiz = "Quiz2";

        TeleportPlayer(QuizTwoTeleportTarget);
    }
    public void GoQuizThree()
    {
        activeQuiz = "Quiz3";

        TeleportPlayer(QuizThreeTeleportTarget);
    }
    public void GoQuizFour()
    {
        activeQuiz = "Quiz4";

        TeleportPlayer(QuizFourTeleportTarget);
    }

    public void GoGarbageCollection()
    {
        if (garbageTeleportTarget.Length == 0)
        {
            Debug.LogError("Garbage teleport array empty!");
            return;
        }

        // Step 1 — Pick random teleport point
        int randomIndex = Random.Range(0, garbageTeleportTarget.Length);
        Transform selectedPoint = garbageTeleportTarget[randomIndex];

        Debug.Log("Selected Garbage Zone: " + selectedPoint.name);

        // Step 2 — Teleport player
        TeleportPlayer(selectedPoint);

        // Step 3 — Get GarbageZone component from parent
        GarbageZone zone = selectedPoint.GetComponentInParent<GarbageZone>();

        if (zone == null)
        {
            Debug.LogError("No GarbageZone script found on parent of: " + selectedPoint.name);
            return;
        }

        // Step 4 — Copy list
        currentGarbageList = new List<Transform>(zone.garbageObjects);

        Debug.Log("Garbage objects loaded: " + currentGarbageList.Count);
    }

    public void GoGhatShahiSnan()
    {
        if (ghatShahiSnanTeleportTarget.Length == 0)
        {
            Debug.LogError("Ghat teleport array empty!");
            return;
        }

        int randomIndex = Random.Range(0, ghatShahiSnanTeleportTarget.Length);
        TeleportPlayer(ghatShahiSnanTeleportTarget[randomIndex]);
    }

    public void GoFireEmergencyHelp()
    {
        activeHelp = "Fire";
        TeleportPlayer(fireEmergencyHelpTeleportTarget);
    }
    /*  public void GoMedicalEmergencyHelp()
      {
          activeHelp = "Medical";

          Debug.Log("Medical Emergency NPC Found: " + npcList.Count);

          NPCController[] allNPCs = FindObjectsByType<NPCController>(FindObjectsSortMode.None);
          npcList.Clear();

          foreach (NPCController npc in allNPCs)
          {
              if (npc.medEmergency)
                  npcList.Add(npc);
          }

          Debug.Log("Medical Emergency NPC Found: " + npcList.Count);

          if (npcList.Count == 0)
          {
              GameObject player = GameObject.FindGameObjectWithTag("Player");

              Debug.LogWarning("No Medical NPC found!");
              taskPopupScreen.SetActive(false);

              CharacterController cc = player.GetComponent<CharacterController>();
              if (cc != null) cc.enabled = true;

              if (cc == null)
              {
                  Debug.LogError("Player or Teleport Target Missing!");
              }
              return;
          }


          int randomIndex = Random.Range(0, npcList.Count);
          Transform npcTransform = npcList[randomIndex].transform;
          Debug.Log("Selected Medical NPC: " + npcTransform.name);

          float spawnRadius = 0f;
          Vector3 randomDir = Random.insideUnitSphere;
          randomDir.y = 0f;
          randomDir.Normalize();

          Vector3 spawnPos = npcTransform.position + randomDir * spawnRadius;
          Quaternion spawnRot = npcTransform.rotation;

          GameObject temp = new GameObject("TempMedicalTeleport");
          temp.transform.position = spawnPos;
          temp.transform.rotation = spawnRot;

          TeleportPlayer(temp.transform);

          Destroy(temp, 0.1f);

          TeleportPlayer(medicalEmergencyHelpTeleportTarget);

          Debug.Log($"Player teleported near Medical NPC: {npcTransform.name}");
      }*/

    public void GoMedicalEmergencyHelp()
    {
        activeHelp = "Medical";

        // 1. Find all NPCs with medicalEmergency enabled
        NPCController[] allNPCs = FindObjectsByType<NPCController>(FindObjectsSortMode.None);
        npcList.Clear();

        foreach (NPCController npc in allNPCs)
        {
            if (npc.medEmergency)
                npcList.Add(npc);
        }

        Debug.Log("Medical Emergency NPC Found: " + npcList.Count);

        // 2. If no NPC found → exit safely
        if (npcList.Count == 0)
        {
            Debug.LogWarning("No Medical Emergency NPC found!");
            taskPopupScreen.SetActive(false);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;

            return;
        }

        // 3. Pick a random NPC
        int randomIndex = Random.Range(0, npcList.Count);
        Transform npcTransform = npcList[randomIndex].transform;

        Debug.Log("Selected Medical NPC: " + npcTransform.name);

        // 4. Choose a safe teleport radius around the NPC
        float spawnRadius = 2f;
        Vector3 randomDir = Random.insideUnitSphere;
        randomDir.y = 0;
        randomDir.Normalize();

        Vector3 spawnPos = npcTransform.position + randomDir * spawnRadius;
        Quaternion spawnRot = Quaternion.LookRotation(npcTransform.position - spawnPos);

        // 5. TEMP teleport target (auto destroy)
        GameObject temp = new GameObject("TempMedicalTeleport");
        temp.transform.position = spawnPos;
        temp.transform.rotation = spawnRot;

        // ✔ Only 1 teleport
        TeleportPlayer(temp.transform);

        Destroy(temp, 0.1f);

        Debug.Log($"Player safely teleported near Medical NPC: {npcTransform.name}");
    }


    public void GoLostItemsSupport()
    {
        Debug.Log("GoLostItemsSupport is call ...");

        activeHelp = "Police";
        TeleportPlayer(lostItemsPeopleSupportTeleportTarget);

        NPCController[] allNPCs = FindObjectsByType<NPCController>(FindObjectsSortMode.None);
        npcList.Clear();

        foreach (NPCController npc in allNPCs)
        {
            if (npc.isRobbed)
                npcList.Add(npc);
        }

        Debug.Log("Lost Item NPC Found: " + npcList.Count);

        if (npcList.Count == 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Debug.LogWarning("No Medical NPC found!");
            taskPopupScreen.SetActive(false);

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;

            if (cc == null)
            {
                Debug.LogError("Player or Teleport Target Missing!");
            }
            return;
        }

        int randomIndex = Random.Range(0, npcList.Count);

        // ✔ FIXED NAME — avoid shadowing
        Transform npcTransform = npcList[randomIndex].transform;

        Debug.Log("Selected Lost Item NPC: " + npcTransform.name);

        float spawnRadius = 2f;

        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0f;
        randomDirection.Normalize();

        Vector3 spawnPosition = npcTransform.position + randomDirection * spawnRadius;
        Quaternion spawnRotation = npcTransform.rotation;

        GameObject tempGO = new GameObject("TempLostItemTeleport");
        Transform temp = tempGO.transform;
        temp.position = spawnPosition;
        temp.rotation = spawnRotation;

        TeleportPlayer(temp);

        Destroy(tempGO, 0.1f);

        Debug.Log($"Player teleported near NPC: {npcTransform.name} at distance {spawnRadius}.");
    }
    public void GoPeopleSupportSupport()
    {
        activeHelp = "LostItem";
        TeleportPlayer(peopleSupportTeleportTarget);

        // Step 2 — Find all NPCs
        NPCController[] allNPCs = FindObjectsByType<NPCController>(FindObjectsSortMode.None);
        npcList.Clear();

        foreach (NPCController npc in allNPCs)
        {
            if (npc.isLost)
                npcList.Add(npc);
        }

        Debug.Log("NPC Found: " + npcList.Count);

        // Step 3 — If no NPC found, do nothing
        if (npcList.Count == 0)
        {
            Debug.LogWarning("No NPC found in scene!");
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            Debug.LogWarning("No Medical NPC found!");
            taskPopupScreen.SetActive(false);

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = true;

            if (cc == null)
            {
                Debug.LogError("Player or Teleport Target Missing!");
            }

            return;
        }

        // Step 4 — Pick random NPC
        int randomIndex = Random.Range(0, npcList.Count);

        // ✔ FIX: renamed 'npc' to avoid name conflict
        Transform targetNPC = npcList[randomIndex].transform;

        Debug.Log("Selected NPC for teleport: " + targetNPC.name);

        float spawnRadius = 2f;

        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0f;
        randomDirection = randomDirection.normalized;

        Vector3 spawnPosition = targetNPC.position + randomDirection * spawnRadius;
        Quaternion spawnRotation = targetNPC.rotation;

        GameObject tempGO = new GameObject("TempPeopleSupportTeleport");
        Transform temp = tempGO.transform;
        temp.position = spawnPosition;
        temp.rotation = spawnRotation;

        TeleportPlayer(temp);

        Destroy(tempGO, 0.1f);

        Debug.Log($"Player teleported near NPC: {targetNPC.name} at distance {spawnRadius}.");
    }
    public void OnQuizFinished()
    {
        Debug.Log("Quiz completed → Updating UI color...");

        switch (activeQuiz)
        {
            case "Quiz1":
                quizOneButton.color = Color.green;
                quizOne.interactable = false;
                break;

            case "Quiz2":
                quizTwoButton.color = Color.green;
                quizTwo.interactable = false;
                break;

            case "Quiz3":
                quizThreeButton.color = Color.green;
                quizThree.interactable = false;
                break;

            case "Quiz4":
                quizFourButton.color = Color.green;
                quizFour.interactable = false;
                break;
        }

        activeQuiz = "";

        completedTaskCount++;
        CheckCompletionStatus();

    }

    public void OnHelpCompleted()
    {
        Debug.Log("Help task completed → updating UI color...");

        switch (activeHelp)
        {
            case "Medical":
                medicalHelpButton.color = Color.green;
                medicalHelp.interactable = false;
                break;

            case "Fire":
                fireHelpButton.color = Color.green;
                fireHelp.interactable = false;
                break;

            case "Police":
                policeHelpButton.color = Color.green;
                policeHelp.interactable = false;
                break;

            case "LostItem":
                lostItemHelpButton.color = Color.green;
                lostItemHelp.interactable = false;
                break;
        }

        activeHelp = "";

        completedTaskCount++;
        CheckCompletionStatus();
    }

    public void OnGhatCheckpointCompleted(int index)
    {
        Debug.Log("Updating ghat activity button for checkpoint: " + index);

        completedTaskCount++;
        CheckCompletionStatus();

        switch (index)
        {
            case 0:
                if (ghatShahiSnanButton != null)
                    ghatShahiSnanButton.color = Color.green;
                    ghatShahiSnan.interactable = false;

                break;

            case 1:
                if (ghatClothChangeRitualButton != null)
                    ghatClothChangeRitualButton.color = Color.green;
                    ghatClothChangeRitual.interactable = false;
                break;

            case 2:
                if (ghatDivineAartiRitualFlowerOfferingRitualButton != null)
                    ghatDivineAartiRitualFlowerOfferingRitualButton.color = Color.green;
                    ghatDivineAartiRitualFlowerOfferingRitual.interactable = false;
                break;
        }
    }

    public void OnAllGhatActivitiesCompleted()
    {
       /* completedTaskCount = completedTaskCount + 3;
        CheckCompletionStatus();
*/
        ghatShahiSnan.interactable = false;
        ghatDivineAartiRitualFlowerOfferingRitual.interactable = false;
        ghatClothChangeRitual.interactable = false;

        Debug.Log("All ghat activities completed! All buttons are green.");

        ghatShahiSnanButton.color = Color.green;
        ghatClothChangeRitualButton.color = Color.green;
        ghatDivineAartiRitualFlowerOfferingRitualButton.color = Color.green;

        ghatActivityCompleted = true;

    }

    public void OngrabgeCompleted()
    {
        garbageIconImage.color = Color.green;
        garbageIcon.interactable=false;
        completedTaskCount++;
        CheckCompletionStatus();
    }

    public void CheckCompletionStatus()
    {
        if (completedTaskCount >= totalTasks && !finalCompletedShown)
        {
            finalCompletedShown = true;
            ShowAllCompletedMessage();
        }
    }

    public void ShowAllCompletedMessage()
    {  
        congratulationsPanel.SetActive(true);
        Debug.Log("🎉 ALL TASKS COMPLETED!");
    }

    public void AllTaskCompletedCloseBtnClick()
    {
        congratulationsPanel.SetActive(false);
    }  

}