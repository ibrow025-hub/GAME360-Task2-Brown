using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs & Points")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Waves")]
    public float intermission = 2f;       // pause between waves
    public int startCount = 5;            // enemies in wave 1
    public int countPerWave = 2;          // extra per wave

    private int waveIndex = 0;
    private bool spawning = false;

    private void Update()
    {
        if (GameManager.Instance.lives <= 0) return;

        // If no enemies alive and not currently spawning, start next wave
        if (!spawning && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private System.Collections.IEnumerator SpawnWave()
    {
        spawning = true;
        waveIndex++;

        // Optional: show "Wave X" with TMP on screen via UI if you want

        yield return new WaitForSeconds(intermission);

        int count = startCount + (waveIndex - 1) * countPerWave;

        for (int i = 0; i < count; i++)
        {
            if (enemyPrefab && spawnPoints.Length > 0 && GameManager.Instance.lives > 0)
            {
                int idx = Random.Range(0, spawnPoints.Length);
                Instantiate(enemyPrefab, spawnPoints[idx].position, Quaternion.identity);
            }
            yield return new WaitForSeconds(0.15f); // slight cadence
        }

        spawning = false;
    }
}
