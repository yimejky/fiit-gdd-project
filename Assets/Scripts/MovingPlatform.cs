using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public int activePointIndex;
    public GameObject movingPoints;
    public GameObject body;
    public bool circularMovement;
    private List<Transform> points = new List<Transform>();
    private int indexStep = 1;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(body.transform.position, points[activePointIndex].position) < 0.1)
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

        body.transform.position = Vector3.MoveTowards(body.transform.position, points[activePointIndex].position, speed * Time.deltaTime);
    }
}
