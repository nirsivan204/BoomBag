using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject characterSelect;
    
    // Start is called before the first frame update
    void Start()
    {
        MainMenuButton();
    }

    public void PlayNowButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("InitialTestScene");
    }

    public void SelectCharacterButton()
    {
        // Character Select button has been pressed, allowing the player to select their character
        mainMenu.SetActive((false));
        creditsMenu.SetActive((false));
        characterSelect.SetActive((true));
    }

    public void CreditsButton()
    {
        // Show Credits Menu
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        characterSelect.SetActive((false));
    }

    public void MainMenuButton()
    {
        // Show Main Menu
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        characterSelect.SetActive((false));
    }
}