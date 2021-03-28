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
    private GameObject winner;

    private float findMiddleX()
    {
        float mostRight = 0;//-1000;
        float mostLeft = 1000;
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
                //if (mostRight < gm.players[i].transform.position.x)
                //{
                player = gm.players[i];
                count++;
                mostRight += gm.players[i].transform.position.x;
                //}

                //if (mostLeft > gm.players[i].transform.position.x)
                //{
                 //   mostLeft = gm.players[i].transform.position.x;
               // }
            }

        }
        if (count == 1)
        {
            winner = player;


        }

        return mostRight / count;

    }
    
    private float findMiddleZ()
    {
        float mostUp = 0;//-1000;
        float mostDown = 1000;
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
                //if (mostUp < gm.players[i].transform.position.z)
                //{
                player = gm.players[i];
                count++;
                mostUp += gm.players[i].transform.position.z;

                //}

                //if (mostDown > gm.players[i].transform.position.z)
                //{
                //    mostDown = gm.players[i].transform.position.z;
                //}
            }
        }
        if (count <= 1)
        {
            winner = player;


        }

        return (mostUp / count); //+ mostDown) / 2;

    }

    private void Start()
    {
        cameraInitialPosition = transform.position;
        Invoke("zoom", updateZoomDelay);
    }

    private void Update()
    {
        transform.position = new Vector3(cameraInitialPosition.x + findMiddleX(), cameraInitialPosition.y + (zoomFactor * zoomDir), cameraInitialPosition.z + findMiddleZ());
        //Vector3 screenPoint = Camera.WorldToViewportPoint(targetPoint.position);
        //bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
    int counter = 0;
    private void zoom()
    {
        Invoke("zoom", updateZoomDelay);
        foreach (AbstractPlayer player in gm.playersScripts)
        {
            if (!player.getIsOut() && !player.isInFrame(desiredMergin))
            {
/*                if (zoomDir < 0)
                {
                    zoomDir = 0;
                }*/
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
/*        if (zoomDir > 0)
        {
            zoomDir = 0;
        }*/
        //zoomDir--;
    }

}
