using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public GameManager gm;
    private Vector3 cameraInitialPosition;
    private float zoomFactor = 0.01f;
    private int zoomDir = 0;

    float desiredMergin = 0.1f;
    float updateZoomDelay = 0.01f;
    int minZoom = -500;
    int maxZoom = 800;
    float cameraSpeed = 50;
    private GameObject winner;

    private float findMiddleX()
    {
        float avrX = 0;
        int count = 0;
        int i = 0;
        GameObject player = null;
        if (winner)
        {
            return winner.transform.position.x;
        }
        for (; i < gm.players.Length; i++)
        {
            if (!gm.playersScripts[i].getIsOut())
            {
                player = gm.players[i];
                count++;
                avrX += gm.players[i].transform.position.x;
            }

        }
        if (count == 1)
        {
            winner = player;


        }

        return avrX / count;

    }
    
    private float findMiddleZ()
    {
        float avgZ = 0;
        int count = 0;
        int i = 0;
        GameObject player = null;
        if (winner)
        {
            return winner.transform.position.z;
        }
        for ( ; i < gm.players.Length ;  i++) 
        {
            if (!gm.playersScripts[i].getIsOut())
            {
                player = gm.players[i];
                count++;
                avgZ += gm.players[i].transform.position.z;
            }
        }
        if (count <= 1)
        {
            winner = player;


        }

        return (avgZ / count);

    }

    private void Start()
    {
        cameraInitialPosition = transform.position;
        Invoke("zoom", updateZoomDelay);
    }

    private void FixedUpdate()
    {
        Vector3 target = new Vector3(cameraInitialPosition.x + findMiddleX(), cameraInitialPosition.y + (zoomFactor * zoomDir), cameraInitialPosition.z + findMiddleZ());
        transform.position = Vector3.MoveTowards(transform.position, target, cameraSpeed * Time.fixedDeltaTime);
    }
    int counter = 0;
    private void zoom()
    {
        Invoke("zoom", updateZoomDelay);
        foreach (AbstractPlayer player in gm.playersScripts)
        {
            if (!player.getIsOut() && !player.isInFrame(desiredMergin))
            {
                if (zoomDir < maxZoom)
                {
                    zoomDir++;
                    return;
                }

            }
        }
        if(zoomDir > minZoom)
        {
                zoomDir--;
        }
    }

}
