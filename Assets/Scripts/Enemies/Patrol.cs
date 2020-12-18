using System;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public bool patrolEnabled = true;
    public float speed = 75;
    public float returnDistance = 5;

    private List<Vector3> points = new List<Vector3>();
    private int activePointIndex = 0;
    private int indexStep = 1;
    private float pointsDistance = 0;
    private GameObject enemy;
    private Transform enemyTrans;

    // Start is called before the first frame update
    void Start()
    {
        enemyTrans = transform.parent;
        enemy = enemyTrans.gameObject;
        Debug.Log($"patrol start {enemyTrans}, {enemy}");

        pointsDistance = returnDistance;
        Transform[] mp = transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < mp.Length; i++)
        {
            points.Add(mp[i].position);
        }

        if (activePointIndex == 0)
        {
            indexStep = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(enemyTrans.position, points[activePointIndex]) < 0.1)
        {
            if (activePointIndex == points.Count - 1 || activePointIndex == 0)
            {
                indexStep *= -1;
            }
            activePointIndex += indexStep;
            pointsDistance = returnDistance;
        }

        //if (!patrolEnabled)
        //{
        //    float positionX = enemyTrans.position.x;
        //    float minDistance = 99999;
        //    points.ForEach(element => minDistance = Math.Min(Math.Abs(element.x - positionX), minDistance));
        //    pointsDistance = minDistance;

        //    if (pointsDistance > returnDistance)
        //    {
        //        patrolEnabled = true;
        //    }
        //}
    }

    private void FixedUpdate()
    {
        if (patrolEnabled)
        {
            // Debug.Log($"patrol {patrolEnabled}");
            patrol();
        }
    }

    private void patrol()
    {
        // Debug.Log($"patrol call: {speed} {enemyTrans.position}, {activePointIndex}, {points[activePointIndex]}");
        enemy.GetComponent<Enemy>().FixedMoveToTarget(points[activePointIndex], speed);
    }

    public void setPatrolEnabled(bool value)
    {
        if (pointsDistance > returnDistance)
        {
            Debug.Log("Not setting patrolEnabled, min distance: " + pointsDistance);
            return;
        }
        Debug.Log($"Setting patrol mode {value}");
        patrolEnabled = value;
    }
}
