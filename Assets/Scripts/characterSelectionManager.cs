using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelectionManager : MonoBehaviour
{
    public CharacterCube[] characterCubes;
    private int numOfCharactersChosen = 0;
    // Start is called before the first frame update

    void Start()
    {
        foreach (CharacterCube cube in characterCubes)
        {
            cube.init();
            cube.characterSelectedEvent.AddListener(characterSelected);
        }
    }
    // Update is called once per frame
    void characterSelected()
    {
        numOfCharactersChosen++;
        if(numOfCharactersChosen == characterCubes.Length)
        {
            LevelManager.getInstance().loadScene(LevelManager.Scenes.Game);
        }
    }

    void characterNotSelected()
    {
        numOfCharactersChosen--;
    }
}
