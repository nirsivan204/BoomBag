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

    public enum ArenaTypes { CHEERIOS, UGI, FLAT }
    public ArenaTypes arena;
    public bool isTilting;
    public GameObject[] arenas;

    public enum CharTypes { Rigid, Soft, Jumper, Avoider };
    public CharTypes[] charTypes;
    public GameObject colorChanger;
    public AbstractPlayer[] playersScripts;
    private int numPlayersAlive;
    public bool[] liveOrDead;
    public IntEvent winEvent;
    public bool[] humanOrAI;
    public GameObject dummyPlayer;
    public UIManager UIMgr;
    [SerializeField] bool colorChangerEnable;
    public int startCountTime = 3;
    public GameObject milk;
    public bool isMilkRising = false;
    public const float MILK_RISE_RATE = 1;
    public const float MILK_RISE_SPEED = 2;
    private Vector3 milkTarget;
    // Start is called before the first frame update
    void Awake()
    {
        arenas[0].SetActive(false);
        numPlayersAlive = 0;
        winEvent = new IntEvent();
        playersScripts = new AbstractPlayer[players.Length];
        liveOrDead = new bool[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
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
                    playerScript = players[i].AddComponent<AvoiderPlayer>();
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
        dummyPlayer.SetActive(false);
        initArena();
    }

    private void initArena()
    {
        GameObject arenaChosen = null;
        switch (arena)
        {
            case ArenaTypes.CHEERIOS:
                arenaChosen = arenas[0];
                break;
            case ArenaTypes.UGI:
                arenaChosen = arenas[1];
                break;
            case ArenaTypes.FLAT:
                arenaChosen = arenas[2];
                break;
        }
        arenaChosen.SetActive(true);
        if (isTilting)
        {
            arenaChosen.GetComponent<TiltManager>().enabled = true;
        }
    }
    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(true);
            playersScripts[i].playerOut.AddListener(playerDied);
        }
        UIMgr.startCounter(startCountTime, "GO!!!!", true, startGame);
    }


    private void startGame()
    {
        colorChanger.SetActive(colorChangerEnable);
        for (int i = 0; i < players.Length; i++)
        {
            playersScripts[i].setCanMove(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isMilkRising)
        {
            float step = MILK_RISE_SPEED * Time.deltaTime; // calculate distance to move
            milk.transform.position = Vector3.MoveTowards(milk.transform.position, milkTarget, step);
            if (Vector3.Distance(milk.transform.position, milkTarget) < 0.001f)
            {
                isMilkRising = false;
                print("here");
            }
        }

    }

    private void playerDied(int playerIndex)
    {
        numPlayersAlive--;
        liveOrDead[playerIndex] = false;
        if (numPlayersAlive == 1)
        {
            for (int i=0; i<players.Length ; i++)
            {
                if (liveOrDead[i])
                {
                    winEvent.Invoke(i+1);
                }
            }
        }
    }

    public void milkRiseStart()
    {
        isMilkRising = true;
        milkTarget = milk.transform.position + Vector3.up * MILK_RISE_RATE;
        print("milkTarget" + milkTarget);
        print("milk");
    }

    private void milkRiseStop()
    {
        isMilkRising = false;
    }

    public Vector3 calculateTorque(Vector3 point)
    {
        Vector3 result = Vector3.zero;
        foreach(AbstractPlayer player in playersScripts)
        {
            if (player && !player.getIsOut())
            {
                result += (player.transform.position-point) * player.getMass();
            }
        }
        return result;
    }

}
