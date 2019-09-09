using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Configuration")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawn = .5f;
    [SerializeField] float spawnRandomFactor = .3f;
    [SerializeField] int numberOfEnemies;
    [SerializeField] float moveSpeed = .3f;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public List<Transform> GetWaypoint() {

        var waveWaypoints = new List<Transform>();

        foreach(Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }

        return waveWaypoints;
    }
    public float GetTimeBetweenSpawn() { return timeBetweenSpawn;  }
    public float GetSwpawnRandomFactor() { return spawnRandomFactor;  }
    public int GetNumberOfEnemies() { return numberOfEnemies;  }
    public float GetMoveSpeed() { return moveSpeed;  }

}
