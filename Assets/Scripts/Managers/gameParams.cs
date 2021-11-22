using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class gameParams
{
    static Dropdown m_Dropdown;
    public static Text m_Text;
    private static bool isMobile;
    public static bool isInit = false;
    public static int numOfRounds { get; set; } = 2;
    public static int roundNumber { get; set; } = 1;

    public static GameManager.CharTypes[] characterArray { get; set; } = { 0, 0, 0, 0 };
    public static GameManager.ArenaTypes arena { get; set; } = 0;
    public static int[] isHumansArray { get; set; }

    public static int[] scores { get; set; } = { 0, 0, 0, 0 };
    static public void init()
    {
        isInit = true;
        scores =  new int[]{ 0, 0, 0, 0 };
        roundNumber = 1;
        initRound();
        //Add listener for when the value of the Dropdown changes, to take action
    }

    static public void initRound()
    {
        arena = (GameManager.ArenaTypes)Random.Range(0, GameManager.numOfArenas);
        chooseRandomCharTypes();
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