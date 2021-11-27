using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject characterSelect;

    public GameObject howToPlay;
    public TMP_Dropdown m_Dropdown;
    public static TextMeshPro m_Text;
    private bool isMobile;
    // Start is called before the first frame update
    void Start()
    {
        gameParams.init();
        //Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });

        //Initialise the Text to say the first value of the Dropdown
        //m_Text.text = "First Value : " + m_Dropdown.value; MainMenuButton();
    }

    private void DropdownValueChanged(TMP_Dropdown m_Dropdown)
    {
        Debug.Log("char " + (GameManager.CharTypes)m_Dropdown.value);
        gameParams.characterArray.SetValue( (GameManager.CharTypes)m_Dropdown.value,0);
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        gameParams.init();
        UnityEngine.SceneManagement.SceneManager.LoadScene("InitialTestScene");
    }

    public void SelectCharacterButton()
    {
        // Character Select button has been pressed, allowing the player to select their character
        mainMenu.SetActive((false));
        creditsMenu.SetActive((false));
        characterSelect.SetActive((true));
        howToPlay.SetActive((false));
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        characterSelect.SetActive((false));
        howToPlay.SetActive((false));
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        characterSelect.SetActive((false));
        howToPlay.SetActive((false));
    }

    public void HowToPlayButton()
    {
        // Shows how to play the game
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        characterSelect.SetActive((false));
        howToPlay.SetActive((true));
    }
}