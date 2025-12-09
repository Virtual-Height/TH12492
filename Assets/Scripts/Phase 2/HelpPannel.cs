using UnityEngine;
using UnityEngine.UI;

public class HelpPannel : MonoBehaviour
{
    public static HelpPannel instance;

    public Button[] options;

    [Header("Medical Team Settings")]
    public GameObject[] medicalPrefabs;  // Doctor, Nurse, etc.
    public GameObject[] medicalVehicles; // Ambulance, etc.
    public float medicalSpawnRadius = 3f;

    [Header("Police Team Settings")]
    public GameObject[] policePrefabs;   // Officer(s)
    public GameObject[] policeVehicles;  // Police Car, Jeep, etc.
    public float policeSpawnRadius = 3f;

    private void Awake()
    {
        instance = this;
    }

    public void SetupOptions(int option)
    {
        string currentHelp = UIManager.Instance.activeHelp;

        for (int i = 0; i < options.Length; i++)
        {
            int index = i;
            options[index].onClick.RemoveAllListeners();

            if (index == option)
            {
                options[index].onClick.AddListener(() =>
                {  
                    Debug.Log($"✅ Correct option pressed (index {index})");
                    UIManager.Instance.OnHelpCompleted();

                    GameManager.instance.messageText.text = "Correct";
                    GameManager.instance.popupPannel.SetActive(true);

                    if (GameManager.instance.selectedAI != null)
                    {
                        // Spawn support team first
                        if (index == 0)
                        {
                            SpawnMedicalTeamNearPlayer();
                        }
                        else if (index == 1)
                        {
                            SpawnPoliceTeamNearPlayer();
                        }

                        // Destroy selected AI after 5 seconds
                        GameManager.instance.selectedAI.GetComponent<CapsuleCollider>().enabled = false;
                        Destroy(GameManager.instance.selectedAI.gameObject, 5f);
                    }

                    GameManager.instance.selectedAI = null;
                    GameManager.instance.AddPoint(50);
                });
            }
            else
            {
                options[index].onClick.AddListener(() =>
                {
                    GameManager.instance.messageText.text = "Incorrect";
                    GameManager.instance.popupPannel.SetActive(true);

                    if (GameManager.instance.selectedAI != null)
                    {
                        GameManager.instance.selectedAI.GetComponent<CapsuleCollider>().enabled = false;
                        Destroy(GameManager.instance.selectedAI.gameObject, 5f);
                    }

                    GameManager.instance.selectedAI = null;
                });
            }
        }

        if (option == 2)
        {
            UIManager.Instance.activeHelp = "Fire";
            options[2].onClick.AddListener(() =>
            {
                GameManager.instance.popupPannel.SetActive(false);
                GameManager.instance.firePannel.SetActive(true);
                GameManager.instance.AddPoint(-50);
            });
        }
    }

    private void SpawnMedicalTeamNearPlayer()
    {
        Transform player = GameManager.instance.activePlayer?.transform;
        if (player == null) return;

        // doctor/nurse
        GameObject medic = null;
        if (medicalPrefabs.Length > 0)
        {
            Vector3 personPos = GetRandomPositionNear(player.position, medicalSpawnRadius);
            medic = Instantiate(medicalPrefabs[Random.Range(0, medicalPrefabs.Length)], personPos, Quaternion.identity);
            medic.transform.LookAt(player);
        }

        // ambulance
        GameObject vehicle = null;
        if (medicalVehicles.Length > 0)
        {
            Vector3 vehiclePos = GetRandomPositionNear(player.position, medicalSpawnRadius + 2f);
            vehicle = Instantiate(medicalVehicles[Random.Range(0, medicalVehicles.Length)], vehiclePos, Quaternion.identity);
            vehicle.transform.LookAt(player);
        }

        // Destroy both medic + vehicle when selected AI is destroyed
        if (GameManager.instance.selectedAI != null)
        {
            float delay = 5f;
            if (medic) Destroy(medic, delay);
            if (vehicle) Destroy(vehicle, delay);
        }
    }

    private void SpawnPoliceTeamNearPlayer()
    {
        
        Transform player = GameManager.instance.activePlayer?.transform;
        if (player == null) return;

        GameObject officer = null;
        if (policePrefabs.Length > 0)
        {
            Vector3 officerPos = GetRandomPositionNear(player.position, policeSpawnRadius);
            officer = Instantiate(policePrefabs[Random.Range(0, policePrefabs.Length)], officerPos, Quaternion.identity);
            officer.transform.LookAt(player);
        }

        GameObject vehicle = null;
        if (policeVehicles.Length > 0)
        {
            Vector3 vehiclePos = GetRandomPositionNear(player.position, policeSpawnRadius + 2f);

            // ✅ Force Y position to 28
            vehiclePos.y = 28f;

            // ✅ Spawn vehicle with rotation (90, 0, 0)
            Quaternion fixedRotation = Quaternion.Euler(90f, 0f, 0f);

            vehicle = Instantiate(
                policeVehicles[Random.Range(0, policeVehicles.Length)],
                vehiclePos,
                fixedRotation,
                transform
            );

            if (GameManager.instance.selectedAI != null)
            {
                float delay = 5f;
                if (officer) Destroy(officer, delay);
                if (vehicle) Destroy(vehicle, delay);
            }
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