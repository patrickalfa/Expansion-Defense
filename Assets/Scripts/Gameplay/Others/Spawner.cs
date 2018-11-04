using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WAVE
{
    public int enemyCount;
    public float timeBetweenSpawns;
    public int amountPerSpawn;
}

public class Spawner : MonoBehaviour
{
    public GameObject[] pfEnemies;
    public WAVE[] waves;
    public int waveIndex;

    private static Spawner m_instance;
    public static Spawner instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<Spawner>();
            return m_instance;
        }
    }

    public void SpawnWave()
    {
        StartCoroutine(WaitForWave());
    }

    private IEnumerator WaitForWave()
    {
        WAVE w = waves[waveIndex];
        int count = w.amountPerSpawn;

        while (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = Random.insideUnitCircle.normalized * Random.Range(10f, 12f);
                Instantiate(pfEnemies[Random.Range(0, pfEnemies.Length)],
                            pos, Quaternion.identity, transform);

            }

            w.enemyCount -= count;

            count = (w.amountPerSpawn > w.enemyCount) ? w.enemyCount : w.amountPerSpawn;

            yield return new WaitForSeconds(w.timeBetweenSpawns);
        }
    }
}
