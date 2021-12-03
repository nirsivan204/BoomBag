using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class gameParams
{
    public static Text m_Text;
    public static bool isMobile;
    public static bool isInit = false;
    public static int numOfRounds { get; set; } = 5;
    public static int roundNumber { get; set; } = 1;
    public static int BGMusic { get; set; } = 0;
    public static int numBGMusic = 2;

    public static GameManager.CharTypes[] characterArray { get; set; } = { 0, 0, 0, 0 };
    public static GameManager.ArenaTypes arena { get; set; } = 0;
    public static int[] isHumansArray { get; set; }

    public static int maxMobileScore = 0;
    public static int[] scores { get; set; } = { 0, 0, 0, 0 };
    static public void init()
    {
        roundNumber = 1;

        scores = new int[] { 0, 0, 0, 0 };

        isInit = true;
        initRound();

        //Add listener for when the value of the Dropdown changes, to take action
    }

    static public void initRound()
    {
        arena = (GameManager.ArenaTypes)Random.Range(0, GameManager.numOfArenas);
        //Debug.Log(arena);
        if (!isMobile)
        {
            chooseRandomCharTypes();

        }
        BGMusic = Random.Range(0, numBGMusic);
        isInit = true;
    }

    private static void chooseRandomCharTypes()
    {
        for (int i = 0; i < characterArray.Length; i++)
        {
            characterArray[i] = (GameManager.CharTypes)Random.Range(0, GameManager.numOfCharTypes);
        }
    }
}


/*    public static void OnValueChanged()
    {

    }*/

/*static void DropdownValueChanged(Dropdown change)
    {
        if (isMobile)
        {
            Debug.Log("char " + (GameManager.CharTypes)change.value);
            characterArray[0] =(GameManager.CharTypes) change.value;
        }
        else
        {

        }

    }
}

*/