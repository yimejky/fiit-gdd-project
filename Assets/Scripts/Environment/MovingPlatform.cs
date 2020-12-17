using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool circularMovement;
    public int activePointIndex;
    public float speed;
    public GameObject movingPoints;
    public GameObject platform;

    private int indexStep = 1;
    Rigidbody2D rb2d;
    private List<Transform> points = new List<Transform>();


    void Start()
    {
        rb2d = platform.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
        // rb2d.isKinematic = true;
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

        //platform.transform.position = Vector3.MoveTowards(platform.transform.position, points[activePointIndex].position, speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        Vector2 targetPosition = Vector2.MoveTowards(rb2d.position, points[activePointIndex].position, speed * Time.fixedDeltaTime);
        Vector2 vel = rb2d.velocity;
        Vector2 newPos = Vector2.SmoothDamp(rb2d.position, targetPosition, ref vel, 0.01f);
        rb2d.position = newPos;
        platform.transform.position = newPos;
    }
}
