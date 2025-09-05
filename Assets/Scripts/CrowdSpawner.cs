using UnityEngine;
using UnityEngine.AI;

public class CrowdSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject malePrefab;
    public GameObject femalePrefab;

    [Header("Spawn Settings")]
    public int spawnCount = 20;
    public float spawnRadius = 30f;
    public float navMeshSampleDistance = 5f;

    [Header("Spawn Delay")]
    public bool spawnOverTime = false;
    public float spawnInterval = 0.2f;

    private void Start()
    {
        if (spawnOverTime)
            StartCoroutine(SpawnCharactersOverTime());
        else
            SpawnAllCharacters();
    }

    void SpawnAllCharacters()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnCharacter();
        }
    }

    System.Collections.IEnumerator SpawnCharactersOverTime()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnCharacter();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCharacter()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPoint.y = transform.position.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
            GameObject prefabToSpawn = (Random.value > 0.5f) ? malePrefab : femalePrefab;
            Instantiate(prefabToSpawn, hit.position, Quaternion.identity).transform.SetParent(transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
