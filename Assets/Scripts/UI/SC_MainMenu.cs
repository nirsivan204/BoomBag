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
    [SerializeField] GameObject creditsButton;
    [SerializeField] GameObject MobileCharacterSelect;
    [SerializeField] AudioSource AS;

    #endregion Ref
    private bool isPlayingNow = false;
    // Start is called before the first frame update
    void Start()
    {

        creditsButton.SetActive(true);

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
            score.SetText(gameParams.maxMobileScore.ToString());
            MobileCharacterSelect.SetActive(true) ;
        }
        else
        {
            scoreBoard.SetActive(false);
            playButton.SetActive(true);
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
        if (!isPlayingNow) // this is because the button of character selections triggers twice for some reason
        {
            AS.Play();
            isPlayingNow = true;
            if (isMobile)
            {//goto character select
                LevelManager.getInstance().loadScene(LevelManager.Scenes.Game);
                //creditsButton.SetActive(false);
            }
            else
            {
                LevelManager.getInstance().loadScene(LevelManager.Scenes.CharacterSelect);
            }
        }


    }

    public void SelectCharacterButton()
    {
        // Character Select button has been pressed, allowing the player to select their character
        AS.Play();
        mainMenu.SetActive((false));
        creditsMenu.SetActive((false));
        characterSelect.SetActive((true));
        howToPlay.SetActive((false));
        scoreBoard.SetActive(false);
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        AS.Play();
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        characterSelect.SetActive((false));
        howToPlay.SetActive((false));
        scoreBoard.SetActive(false);
        playButton.SetActive(false) ;
    }

    public void MainMenuButton()
    {
        AS.Play();
        // Show Main Menu
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        characterSelect.SetActive((false));
        howToPlay.SetActive((false));
        
        if (isMobile)
        {
            scoreBoard.SetActive(true);
            playButton.SetActive(false);
        }

        else
        {
            scoreBoard.SetActive(false);
            playButton.SetActive(true);
        }
    }

    public void HowToPlayButton()
    {
        // Shows how to play the game
        AS.Play();
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        characterSelect.SetActive((false));
        howToPlay.SetActive((true));
        scoreBoard.SetActive(false);
        playButton.SetActive(false);
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