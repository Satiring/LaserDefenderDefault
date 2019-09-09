using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] [Range(0,5)] float timeBetweenWaves = 0;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
        
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for(int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
           GameObject enemyTemp = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoint()[0].transform.position,
                Quaternion.identity);
            EnemyPathing enemyPathing = enemyTemp.GetComponent<EnemyPathing>();
            enemyPathing.SetWaveConfig(waveConfig);

            // Cutre as fuck
            // Tiempo entre oleadas.
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawn());
        }

    }
    private IEnumerator SpawnAllWaves()
    {
        Debug.Log("Se spawnearan " + (waveConfigs.Count - startingWave) + " de las totales: " + waveConfigs.Count);
        for(int waveIndex = startingWave;waveIndex < waveConfigs.Count; waveIndex++)
        {
            Debug.Log("Wave: " + (waveIndex+1) + "/" + waveConfigs.Count);
            WaveConfig currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
