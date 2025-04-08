using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class verticlaplatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTimeValue = 2f;
    private float waitTime;

    void Start()
    {
        waitTime = waitTimeValue;
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180;
                waitTime = waitTimeValue;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            effector.rotationalOffset = 0;
        }
    }

}
