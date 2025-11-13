/*using UnityEngine;
using UnityEngine.UI;

public class HelpPannel : MonoBehaviour
{
    public static HelpPannel instance;

    public Button[] options;

    [Header("Medical Team Settings")]
    public GameObject[] medicalPrefabs;  // Doctor, Nurse, etc.
    public GameObject[] medicalVehicles; // Ambulance, etc.
    public float medicalSpawnRadius = 3f;
    public float medicalDestroyAfter = 15f;

    [Header("Police Team Settings")]
    public GameObject[] policePrefabs;   // Officer(s)
    public GameObject[] policeVehicles;  // Police Car, Jeep, etc.
    public float policeSpawnRadius = 3f;
    public float policeDestroyAfter = 15f;

    private void Awake()
    {
        instance = this;
    }

    public void SetupOptions(int option)
    {
        for (int i = 0; i < options.Length; i++)
        {
            int index = i; // ✅ Fix lambda capture

            options[index].onClick.RemoveAllListeners();

            if (index == option)
            {
                options[index].onClick.AddListener(() =>
                {
                    Debug.Log($"✅ Correct option pressed (index {index})");

                    GameManager.instance.messageText.text = "Correct";
                    GameManager.instance.popupPannel.SetActive(true);

                    if (GameManager.instance.selectedAI != null)
                    {
                        GameManager.instance.selectedAI.GetComponent<CapsuleCollider>().enabled = false;
                        Destroy(GameManager.instance.selectedAI.gameObject, 5f);
                    }

                    GameManager.instance.selectedAI = null;
                    GameManager.instance.AddPoint(50);

                    if (index == 0)
                    {
                        // 🩺 Spawn Medical team (doctor + ambulance)
                        SpawnMedicalTeamNearPlayer();
                    }
                    else if (index == 1)
                    {
                        // 👮 Spawn Police team (officer + car)
                        SpawnPoliceTeamNearPlayer();
                    }
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

        if (player == null)
        {
            Debug.LogWarning("⚠️ Player reference not found in GameManager!");
            return;
        }

        
        if (medicalPrefabs.Length > 0)
        {
            Vector3 personPos = GetRandomPositionNear(player.position, medicalSpawnRadius);
            int randomIndex = Random.Range(0, medicalPrefabs.Length);
            GameObject medic = Instantiate(medicalPrefabs[randomIndex], personPos, Quaternion.identity);
            medic.transform.LookAt(player);
            Destroy(medic, medicalDestroyAfter);
            Debug.Log($"Spawned Medical Person: {medic.name}");
        }

       
        if (medicalVehicles.Length > 0)
        {
            Vector3 vehiclePos = GetRandomPositionNear(player.position, medicalSpawnRadius + 2f);
            int randomIndex = Random.Range(0, medicalVehicles.Length);
            GameObject vehicle = Instantiate(medicalVehicles[randomIndex], vehiclePos, Quaternion.identity);
            vehicle.transform.LookAt(player);
            Destroy(vehicle, medicalDestroyAfter);
            Debug.Log($" Spawned Ambulance: {vehicle.name}");
        }
    }

   
    private void SpawnPoliceTeamNearPlayer()
    {
        Transform player = GameManager.instance.activePlayer?.transform;

        if (player == null)
        {
            Debug.LogWarning(" Player reference not found in GameManager!");
            return;
        }

        // spawn officer
        if (policePrefabs.Length > 0)
        {
            Vector3 officerPos = GetRandomPositionNear(player.position, policeSpawnRadius);
            int randomIndex = Random.Range(0, policePrefabs.Length);
            GameObject officer = Instantiate(policePrefabs[randomIndex], officerPos, Quaternion.identity);
            officer.transform.LookAt(player);
            Destroy(officer, policeDestroyAfter);
            Debug.Log($" Spawned Police Officer: {officer.name}");
        }

        // spawn police vehicle
        if (policeVehicles.Length > 0)
        {
            Vector3 vehiclePos = GetRandomPositionNear(player.position, policeSpawnRadius + 2f);
            int randomIndex = Random.Range(0, policeVehicles.Length);
            GameObject vehicle = Instantiate(policeVehicles[randomIndex], vehiclePos, Quaternion.identity);
            vehicle.transform.LookAt(player);
            Destroy(vehicle, policeDestroyAfter);
            Debug.Log($" Spawned Police Vehicle: {vehicle.name}");
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
*/

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
        for (int i = 0; i < options.Length; i++)
        {
            int index = i;
            options[index].onClick.RemoveAllListeners();

            if (index == option)
            {
                options[index].onClick.AddListener(() =>
                {
                    Debug.Log($"✅ Correct option pressed (index {index})");

                    GameManager.instance.messageText.text = "Correct";
                    GameManager.instance.popupPannel.SetActive(true);

                    if (GameManager.instance.selectedAI != null)
                    {
                        // Spawn support team first
                        if (index == 0)
                            SpawnMedicalTeamNearPlayer();
                        else if (index == 1)
                            SpawnPoliceTeamNearPlayer();

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
            vehicle = Instantiate(policeVehicles[Random.Range(0, policeVehicles.Length)], vehiclePos, Quaternion.identity);
            vehicle.transform.LookAt(player);
        }

        if (GameManager.instance.selectedAI != null)
        {
            float delay = 5f;
            if (officer) Destroy(officer, delay);
            if (vehicle) Destroy(vehicle, delay);
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
