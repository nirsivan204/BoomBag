using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This does not work currently - however it shows how bad I am at coding, which is remarkable on it's own.

public class UIManager : MonoBehaviour
{
    public TMP_Text winLoseCondition;
    private static bool player1Win; 
    private static bool player2Win;
    private GameObject player1;
    private GameObject player2;
    private GameObject baseGround;

    private void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        baseGround = GameObject.Find("BaseGround");
    }

    void Update()
    
        {
            if (player1.transform.position.y < baseGround.transform.position.y) //Check if Player1 is off the ground
            {
                winLoseCondition.SetText("Player 1 wins!");
            }

            else if (player2.transform.position.y <  baseGround.transform.position.y) // Check if Player2 is off the ground
            {
                winLoseCondition.SetText("Player 2 wins!");
            }

        }
    
}