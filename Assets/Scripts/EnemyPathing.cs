using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    private WaveConfig waveConfig;
    private List<Transform> waypoints;

    private int wayPointIndex = 0;
    private float magicNumber = 2.2f;

    [SerializeField] private Transform actualTarget;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWaypoint();
        if (waypoints != null)
        {
            actualTarget = waypoints[wayPointIndex];
            transform.position = actualTarget.transform.position;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (wayPointIndex <= waypoints.Count -1)
        {
            actualTarget = waypoints[wayPointIndex];
            transform.position = Vector2.MoveTowards
                (transform.position, 
                actualTarget.position,
                waveConfig.GetMoveSpeed()* Time.deltaTime*magicNumber);


            //Debug.Log("Distancia al objetivo: " + Vector2.Distance(transform.position, actualTarget.position));

            if (Vector2.Distance(transform.position, actualTarget.position) <= 0)
            {
                wayPointIndex++;
            }

        }
        else
        {
                Destroy(gameObject, 0);
                        
        }
    }




}
