using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class gameParams
{
    static Dropdown m_Dropdown;
    public static Text m_Text;
    private static bool isMobile;

    public static GameManager.CharTypes[] characterArray { get; set; } = { 0, 0, 0, 0 };
    public static int[] isHumansArray { get; set; }

    static public void init()
    {
        //Add listener for when the value of the Dropdown changes, to take action
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