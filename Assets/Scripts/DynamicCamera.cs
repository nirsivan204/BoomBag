using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public GameManager gm;
    private Vector3 cameraInitialPosition;
    private int zoomFactor = 1;
    private int zoomDir = 0;

    private float findMiddleX()
    {
        float mostRight = -1000;
        float mostLeft = 1000;
        for (int i = 0; i < gm.players.Length; i++)
        {
            if (!gm.playersScripts[i].getIsOut())
            {
                if (mostRight < gm.players[i].transform.position.x)
                {
                    mostRight = gm.players[i].transform.position.x;
                }

                if (mostLeft > gm.players[i].transform.position.x)
                {
                    mostLeft = gm.players[i].transform.position.x;
                }
            }

        }

        return (mostLeft + mostRight) / 2;

    }
    
    private float findMiddleZ()
    {
        float mostUp = -1000;
        float mostDown = 1000;
        for (int i = 0 ; i < gm.players.Length ;  i++) 
        {
            if (!gm.playersScripts[i].getIsOut())
            {
                if (mostUp < gm.players[i].transform.position.z)
                {
                    mostUp = gm.players[i].transform.position.z;
                }

                if (mostDown > gm.players[i].transform.position.z)
                {
                    mostDown = gm.players[i].transform.position.z;
                }
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
        transform.position = new Vector3(cameraInitialPosition.x + findMiddleX(), cameraInitialPosition.y + (zoomFactor * zoomDir), cameraInitialPosition.z + findMiddleZ());
        zoom();
    }

    private void zoom()
    {
        /*foreach (AbstractPlayer player in gm.playersScripts)
        {
            if (!player.getIsOut() && !player.isInFrame())
            {
                if (zoomDir < 0)
                {
                    zoomDir = 0;
                }
                zoomDir++;
                return;
            }
        }
        if (zoomDir > 0)
        {
            zoomDir = 0;
        }
        zoomDir--;*/
    }
}
