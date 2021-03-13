using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;

    public enum CharTypes { Rigid, Soft, Jumper, Avoider };
    public CharTypes[] charTypes;
    public GameObject colorChanger;

    // Start is called before the first frame update
    void Awake()
    {
       // foreach(GameObject player in players)
       // {
       //     player.SetActive(false);
       // }
    }

    void Start()
    {
        for(int i = 0; i<players.Length; i++)
        {
            AbstractPlayer playerScript = null;
            switch (charTypes[i])
            {
                case CharTypes.Rigid:
                    playerScript = players[i].AddComponent<RigidPlayer>();
                    //to do : decide on meshRenderer
                    break;
                case CharTypes.Soft:
                    playerScript = players[i].AddComponent<SoftPlayer>();
                    //to do : decide on meshRenderer

                    break;
                case CharTypes.Jumper:
                    playerScript = players[i].AddComponent<JumperPlayer>();
                    //to do : decide on meshRenderer

                    break;
                case CharTypes.Avoider:
                    //playerScript = players[i].AddComponent<RigidPlayer>();
                    //to do : decide on meshRenderer

                    break;
            }
            if (playerScript)
            {
                playerScript.enabled = true;
                playerScript.setPlayerIndex(i);
                playerScript.setGameManager(this);
                players[i].SetActive(true);
            }
            else
            {
                print("ERROR in creating player " + i);
            }
        }
        colorChanger.SetActive(true);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
