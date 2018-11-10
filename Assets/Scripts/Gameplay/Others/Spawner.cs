using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WAVE
{
    public int enemyCount;
    public float timeBetweenSpawns;
    public int amountPerSpawn;
    public float[] enemiesChance;
}

public class Spawner : MonoBehaviour
{
    public GameObject[] pfEnemies;
    public GameObject pfBoss;
    public WAVE[] waves;
    public int waveIndex;

    public int enemiesAlive;

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
        enemiesAlive = w.enemyCount;
        int count = w.amountPerSpawn;

        while (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                int index = 0;
                float chance = Random.value;
                float curVal = 1f;

                for (int j = 0; j < w.enemiesChance.Length; j++)
                {
                    curVal -= w.enemiesChance[j];
                    if (chance >= curVal)
                    {
                        index = j;
                        break;
                    }
                }

                Vector3 pos = Random.insideUnitCircle.normalized * Random.Range(15f, 17f);
                Instantiate(pfEnemies[index], pos, Quaternion.identity, transform);
            }

            w.enemyCount -= count;

            count = (w.amountPerSpawn > w.enemyCount) ? w.enemyCount : w.amountPerSpawn;

            yield return new WaitForSeconds(w.timeBetweenSpawns);
        }

        waveIndex++;

        if (waveIndex == waves.Length)
        {
            Vector3 pos = Random.insideUnitCircle.normalized * Random.Range(15f, 17f);
            Instantiate(pfBoss, pos, Quaternion.identity, transform);
            enemiesAlive++;
        }
    }
}
