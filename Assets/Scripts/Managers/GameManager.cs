using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class IntEvent : UnityEvent<int>
{
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManagerRef;
    public AudioManager AudioManagerRef
    {
        get
        {
            return audioManagerRef;
        }
    }

    public GameObject[] players;
    public GameObject[] prefabsForPlayers;
    public GameObject pickup;
    public bool createPickups;
    public bool isMobileGame;
    public enum ArenaTypes { CHEERIOS, UGI, FLAT }
    public ArenaTypes arena;
    public bool isTilting;
    public GameObject[] arenas;

    [SerializeField] GameObject mobileCamera;
    [SerializeField] GameObject pcCamera;

    public enum CharTypes { Rigid, Soft, Jumper, Avoider };
    public CharTypes[] charTypes = { CharTypes.Rigid, CharTypes.Soft, CharTypes.Jumper, CharTypes.Avoider };
    public ColorChangeManager colorChanger;
    public AbstractPlayer[] playersScripts;
    private int numPlayersAlive;
    public bool[] liveOrDead;
    public IntEvent winEvent;
    public bool[] humanOrAI;
    public GameObject dummyPlayer;
    public UIManager mobileUIMgr;
    public UIManager PCUIMgr;
    private UIManager UIMgr;
    [SerializeField] bool colorChangerEnable;
    [SerializeField] bool isTeams;

    public int startCountTime = 3;
    public GameObject milk;
    public bool isMilkRising = false;
    public const float MILK_RISE_RATE = 1;
    public const float MILK_RISE_SPEED = 2;
    private Vector3 milkTarget;
    private GameObject arenaChosen = null;
    public GameObject pickupBounderies;
    private float xBoundery;
    private float yBoundery;
    private float zBoundery;
    private AudioSource audioSource;

    public SimpleTouchController touchController;
    public AbstractPlayer mobilePlayer;


    public ParticlesManager PM;
    // Start is called before the first frame update
    void Awake()
    {
        getGameParams();

        arenas[0].SetActive(false);
        numPlayersAlive = 0;
        winEvent = new IntEvent();
        playersScripts = new AbstractPlayer[players.Length];
        liveOrDead = new bool[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
            AbstractPlayer playerScript = null;

            ////////// Set Prefab to Player //////////
            // List of prefab names, and te names of the parts inside them that should change color:
            Dictionary<string, string> prefabToMainPartMap = new Dictionary<string, string>()
            {
                {"the_being", "Chair"},
                {"Mouse3D_10_combinedMeshInSphere_ForUnity", "Bizo_MAIN"},
                {"ss_guppy_no_modifier", "Lifebuoy"},
                {"Concept_CH_Jelly_7_RigOnly_For_Unity", ""},
                {"SlimePBR", "Slime"}
            };
            // Instantiate prefab and set its names to our standards. Then set it to the player (by choosing a parent):
            var pref = Instantiate(prefabsForPlayers[i], new Vector3(0, 0, 0), Quaternion.identity);
            print(pref.name);
            print(prefabsForPlayers[i].name);
            pref.transform.Find(prefabToMainPartMap[prefabsForPlayers[i].name]).name = "Character";
            pref.name = "Body";
            pref.transform.parent = players[i].transform;
            pref.transform.localPosition = new Vector3(0, 0, 0);

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
        if(isMobileGame)
        {
            for(int i = 0; i < humanOrAI.Length;i++) 
            {
                if (humanOrAI[i])
                {
                    mobilePlayer = playersScripts[i];
                    break;
                }
            }
            UIMgr = mobileUIMgr;
            mobileCamera.SetActive(true);
        }
        else
        {
            UIMgr = PCUIMgr;
            pcCamera.SetActive(true);
        }
        UIMgr.gameObject.SetActive(true);
        dummyPlayer.SetActive(false);
        initArena();
    }

    private void getGameParams()
    {
        if (gameParams.isInit)
        {
            charTypes = gameParams.characterArray;

        }
    }

    private void initArena()
    {
        
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
        if (createPickups)
        {
            calculateBounderiesOfPickups();

        }
    }

    private void calculateBounderiesOfPickups()
    {


     //   return center + new Vector3(
     //       (Random.value - 0.5f) * pickupBounderies.transform.localScale.x,
     //       pickupBounderies.transform.position.y,
    //        (Random.value - 0.5f) * pickupBounderies.transform.localScale.z)
    //);
        xBoundery = pickupBounderies.transform.localScale.x;
        yBoundery = pickupBounderies.transform.position.y;
        zBoundery = pickupBounderies.transform.localScale.z;
    }


    void Start()
    {
        AudioManagerRef.Init(this);
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(true);
            playersScripts[i].playerOut.AddListener(playerDied);

        }
        if (isMobileGame && mobilePlayer)
        {
            mobilePlayer.touchController = touchController;
        }
        UIMgr.startCounter(startCountTime, "GO!!!!", true, startGame);
        AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Countdown);
    }


private void startGame()
    {
        colorChanger.gameObject.SetActive(colorChangerEnable);
        colorChanger.isTeams = isTeams;
        colorChanger.UIManager = UIMgr;
        for (int i = 0; i < players.Length; i++)
        {
            StartCoroutine(playersScripts[i].setCanMove(true,0));
        }
        if (createPickups)
        {
            InvokeRepeating("createPickup", 5, 10);
        }
        AudioManagerRef.Play_Sound(AudioManager.SoundTypes.BG_Music);
        // StartCoroutine(MusicUtil.FadeIn(audioSource, 3));
    }
    private Vector3 getRandomLocation()
    {
        return new Vector3((Random.value - 0.5f) * xBoundery, yBoundery, (Random.value - 0.5f) * zBoundery);
    }
    private void createPickup()
    {
        Pickup.pickupsTypes type = Pickup.getRandomType();
        Vector3 randLocation = getRandomLocation();
        GameObject pickupClone = Instantiate(pickup, randLocation,Quaternion.identity);
        Vector3 scaleTmp = pickupClone.transform.localScale;
        scaleTmp.x /= arenaChosen.transform.localScale.x;
        scaleTmp.y /= arenaChosen.transform.localScale.z;
        scaleTmp.z /= arenaChosen.transform.localScale.y;
        pickupClone.transform.parent = arenaChosen.transform;
        pickupClone.transform.localScale = scaleTmp;
        pickupClone.transform.localRotation = Quaternion.Euler(90,0,0);

        Pickup script = pickupClone.GetComponent<Pickup>();
        script.setType(type);
        pickupClone.SetActive(true);
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
        //if(numPlayersAlive == 2 && isTeams)
       // {
       //     colorChanger.isTeams = false;
       // }
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

    public void mobilePlayerPressedAction()
    {
        mobilePlayer.OnFire();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(0);
    }
}
