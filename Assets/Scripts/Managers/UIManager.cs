using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This does not work currently - however it shows how bad I am at coding, which is remarkable on it's own.

public class UIManager : MonoBehaviour
{
    public TMP_Text winText;
    public GameObject baseGround;
    public GameManager gm;

    void Start()
    {
        gm.winEvent.AddListener(setWinText);
    }

    void Update()
    {

    }

    public void setWinText(int winnerIndex)
    {

        winText.SetText("Player "+ winnerIndex + " wins!");
    }

}