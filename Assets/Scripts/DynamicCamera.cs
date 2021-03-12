using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public GameObject[] players;
    public Vector3 cameraInitialPosition;

    private float findMiddleX()
    {
        float mostRight = -1000;
        float mostLeft = 1000;
        foreach (GameObject player in players) 
        {
            if (mostRight < player.transform.position.x)
            {
                mostRight = player.transform.position.x;
            }

            if (mostLeft > player.transform.position.x)
            {
                mostLeft = player.transform.position.x;
            }
        }

        return (mostLeft + mostRight) / 2;

    }
    
    private float findMiddleZ()
    {
        float mostUp = -1000;
        float mostDown = 1000;
        foreach (GameObject player in players) 
        {
            if (mostUp < player.transform.position.z)
            {
                mostUp = player.transform.position.z;
            }

            if (mostDown > player.transform.position.z)
            {
                mostDown = player.transform.position.z;
            }
        }

        return (mostUp + mostDown) / 2;

    }

    private void Start()
    {
        cameraInitialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(cameraInitialPosition.x + findMiddleX(), cameraInitialPosition.y, cameraInitialPosition.z + findMiddleZ());
    }
}
