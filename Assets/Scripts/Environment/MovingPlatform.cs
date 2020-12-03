using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public int activePointIndex;
    public GameObject movingPoints;
    public GameObject platform;
    public bool circularMovement;
    private List<Transform> points = new List<Transform>();
    private int indexStep = 1;

    void Start()
    {
        Transform[] mp = movingPoints.transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < mp.Length; i++)
        {
            points.Add(mp[i]);
        }

        if (activePointIndex == 0)
        {
            indexStep = -1;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(platform.transform.position, points[activePointIndex].position) < 0.1)
        {
            if (circularMovement)
            {
                activePointIndex = (activePointIndex + 1) % points.Count;
            }
            else
            {
                if (activePointIndex == points.Count - 1 || activePointIndex == 0)
                {
                    indexStep *= -1;
                }
                activePointIndex += indexStep;
            }
        }
    }

    void FixedUpdate()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, points[activePointIndex].position, speed * Time.fixedDeltaTime);
    }
}
