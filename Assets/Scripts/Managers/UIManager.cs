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
    public GameObject player1;
    public GameObject player2;
    public GameObject baseGround;



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