using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelectionManager : MonoBehaviour
{
    public CharacterCube[] characterCubes;
    private int numOfCharactersChosen = 0;
    private float allChosenTime;
    private float timeTillStart = 2;
    // Start is called before the first frame update

    void Start()
    {
        foreach (CharacterCube cube in characterCubes)
        {
            cube.init();
            cube.characterSelectedEvent.AddListener(characterSelected);
            cube.characterDeSelectedEvent.AddListener(characterNotSelected);
        }
    }
    // Update is called once per frame
    void characterSelected()
    {
        numOfCharactersChosen++;
        if(numOfCharactersChosen == characterCubes.Length)
        {
            allChosenTime = Time.time;
        }
    }

    void startGame()
    {
        foreach(CharacterCube cube in characterCubes)
        {
            cube.disableInput();
        }
        LevelManager.getInstance().loadScene(LevelManager.Scenes.Game);

    }

    void characterNotSelected()
    {
        numOfCharactersChosen--;
        allChosenTime = 0;
    }

    private void Update()
    {
        if(allChosenTime > 0 && Time.time - allChosenTime > timeTillStart)
        {
            startGame();
            allChosenTime = 0;
        }
    }
}
