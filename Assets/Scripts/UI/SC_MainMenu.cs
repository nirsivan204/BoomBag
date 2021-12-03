using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_MainMenu : MonoBehaviour
{
    #region Ref
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject characterSelect;
    public GameObject scoreBoard;

    public GameObject howToPlay;
    public TMP_Dropdown m_Dropdown;
    [SerializeField] TMP_Text score;
    [SerializeField] bool isMobile;
    [SerializeField] GameObject playButton;
    #endregion Ref
    
    // Start is called before the first frame update
    void Start()
    {
        if (gameParams.isInit)
        {
            isMobile = gameParams.isMobile;
        }
        else
        {
            gameParams.isMobile = isMobile;
        }
        gameParams.init();
        if (isMobile)
        {
            scoreBoard.SetActive(true);
            score.SetText(gameParams.maxMobileScore.ToString());
            playButton.SetActive(true) ;
        }
        else
        {
            playButton.SetActive(true);
            scoreBoard.SetActive(false);
        }

        //Add listener for when the value of the Dropdown changes, to take action
        //m_Dropdown.onValueChanged.AddListener(delegate {
        //    DropdownValueChanged(m_Dropdown);
        //});

        //Initialise the Text to say the first value of the Dropdown
        //m_Text.text = "First Value : " + m_Dropdown.value; MainMenuButton();
    }

    private void DropdownValueChanged(TMP_Dropdown m_Dropdown)
    {
        Debug.Log("char " + (GameManager.CharTypes)m_Dropdown.value);
        gameParams.characterArray.SetValue( (GameManager.CharTypes)m_Dropdown.value,0);
    }
#region Buttons
    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        //gameParams.init();
        if (isMobile)
        {//goto character select
            LevelManager.getInstance().loadScene(LevelManager.Scenes.Game);
        }
        else
        {
            LevelManager.getInstance().loadScene(LevelManager.Scenes.CharacterSelect);
        }

    }

    public void SelectCharacterButton()
    {
        // Character Select button has been pressed, allowing the player to select their character
        mainMenu.SetActive((false));
        creditsMenu.SetActive((false));
        characterSelect.SetActive((true));
        howToPlay.SetActive((false));
        scoreBoard.SetActive(false);
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        characterSelect.SetActive((false));
        howToPlay.SetActive((false));
        scoreBoard.SetActive(false);
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        characterSelect.SetActive((false));
        howToPlay.SetActive((false));
        scoreBoard.SetActive(true);
    }

    public void HowToPlayButton()
    {
        // Shows how to play the game
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        characterSelect.SetActive((false));
        howToPlay.SetActive((true));
        scoreBoard.SetActive(false);
    }
#endregion Buttons
#region ChosenChar
    public void OnRigidChosen()
    {
        gameParams.characterArray[0] = GameManager.CharTypes.Rigid;
        PlayNowButton();
    }
    public void OnGhostChosen()
    {
        gameParams.characterArray[0] = GameManager.CharTypes.Soft;
        PlayNowButton();

    }
    public void OnJumperChosen()
    {
        gameParams.characterArray[0] = GameManager.CharTypes.Jumper;
        PlayNowButton();

    }
    public void OnDasherChosen()
    {
        gameParams.characterArray[0] = GameManager.CharTypes.Avoider;
        PlayNowButton();

    }
#endregion ChosenChar
}