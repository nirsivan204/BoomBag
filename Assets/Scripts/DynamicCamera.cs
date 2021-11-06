using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public GameManager gm;
    [SerializeField] private bool isMobile;
    private Vector3 cameraInitialPosition;
    private float zoomFactor = 0.01f;
    private int zoomDir = 0;

   public float desiredMergin = 0.1f;
   public  float updateZoomDelay = 0.01f;
   public  int minZoom = -500;
   public  int maxZoom = 800;
   public  float cameraSpeed = 50;
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
        if (count == 0)
        {
            return 0;
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
        if (count == 0)
        {
            return 0;
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
        Vector3 target = cameraInitialPosition;
        if (isMobile && gm.mobilePlayer && !gm.mobilePlayer.getIsOut())
        {
            Vector3 mobilePlayerPos = gm.mobilePlayer.transform.position;
            target = cameraInitialPosition + new Vector3(mobilePlayerPos.x, mobilePlayerPos.y + 5, mobilePlayerPos.z);
        }
        else
        {
            target = cameraInitialPosition + new Vector3( findMiddleX(), zoomFactor * zoomDir, findMiddleZ());
        }
        transform.position = Vector3.MoveTowards(transform.position, target, cameraSpeed * Time.fixedDeltaTime);
    }
    private void zoom()
    {
        Invoke("zoom", updateZoomDelay);
        if (!isMobile || gm.mobilePlayer.getIsOut())
        {
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
            if (zoomDir > minZoom)
            {
                zoomDir--;
            }
        }
    }
}
