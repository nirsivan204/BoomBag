using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntEvent : UnityEvent<int>
{
}

public class GameManager : MonoBehaviour
{
    public GameObject[] players;

    public enum CharTypes { Rigid, Soft, Jumper, Avoider };
    public CharTypes[] charTypes;
    public GameObject colorChanger;
    public AbstractPlayer[] playersScripts;
    private int numPlayersAlive;
    public bool[] liveOrDead;
    public IntEvent winEvent;
    public bool[] humanOrAI;
    // Start is called before the first frame update
    void Awake()
    {
        numPlayersAlive = 0;
        winEvent = new IntEvent();
        playersScripts = new AbstractPlayer[players.Length];
        liveOrDead = new bool[players.Length];
        for (int i = 0; i < players.Length; i++)
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
                playerScript.setIsHuman(humanOrAI[i]);
                playerScript.setPlayerIndex(i);
                playerScript.setGameManager(this);
                playersScripts[i] = playerScript;
                playerScript.enabled = true;

            }
            else
            {
                print("ERROR in creating player " + i);
            }
            numPlayersAlive++;
            liveOrDead[i] = true;
        }
        colorChanger.SetActive(true);

    }

    void Start()
    {
        for(int i=0; i< players.Length; i++)
        {
            players[i].SetActive(true);
            playersScripts[i].playerOut.AddListener(playerDied);
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void playerDied(int playerIndex)
    {
        numPlayersAlive--;
        liveOrDead[playerIndex] = false;
        if (numPlayersAlive == 1)
        {
            winEvent.Invoke(playerIndex);
        }
    }
}
