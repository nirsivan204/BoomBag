using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManuController : MonoBehaviour
{
    [SerializeField] SC_MainMenu mainMenu;
    public void OnFire()
    {
        mainMenu.PlayNowButton();
    }
}
