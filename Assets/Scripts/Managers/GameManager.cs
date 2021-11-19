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
    [Header("SETUP")]
    [SerializeField] AudioManager audioManagerRef;
    [SerializeField] GameObject mobileCamera;
    [SerializeField] GameObject pcCamera;
    [SerializeField] SimpleTouchController touchController;
    [SerializeField] ColorChangeManager colorChanger;
    [SerializeField] GameObject milk;
    [SerializeField] ParticlesManager PM;
    [SerializeField] GameObject dummyPlayer;
    [SerializeField] UIManager mobileUIMgr;
    [SerializeField] UIManager PCUIMgr;
    [SerializeField] PickupsManager pickupMgr;



    public float SNOW_HEIGHT;

    public GameObject[] players;
    public GameObject[] prefabsForPlayers;
    public GameObject pickup;
    public bool createPickups;
    public bool isMobileGame;
    public enum ArenaTypes { CHEERIOS, UGI, FLAT }
    public ArenaTypes arena;
    public bool isTilting;
    public GameObject[] arenas;

    public enum CharTypes { Rigid, Soft, Jumper, Avoider };
    public CharTypes[] charTypes = { CharTypes.Rigid, CharTypes.Soft, CharTypes.Jumper, CharTypes.Avoider };
    public AbstractPlayer[] playersScripts;
    private int numPlayersAlive;
    public bool[] liveOrDead;
    public IntEvent winEvent;
    public bool[] humanOrAI;

    private UIManager UIMgr;
    [SerializeField] bool colorChangerEnable;
    [SerializeField] bool isTeams;

    public int startCountTime = 3;

    public bool isSuddenDeath = false;
    private bool isMilkRising = false;
    private int milkRiseCount = 0;

    private Vector3 milkTarget;
    private GameObject arenaChosen = null;
    private float arenaOutRadius;
    private float arenaInnerRadius;
    private AbstractPlayer mobilePlayer;
    private TiltManager tiltMgr;


    // Start is called before the first frame update
    void Awake()
    {
        getGameParams();
        initArena();

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
                {"NewSlime", "NewSlimeBody"}
              // {"SlimePBR", "Slime"}
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
                playerScript.SetArenaRadius(arenaOutRadius);
                playerScript.SetHoleRadius(arenaInnerRadius);
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

        if (isMobileGame)
        {
            for(int i = 0; i < humanOrAI.Length;i++) 
            {
                if (humanOrAI[i])
                {
                    SetMobilePlayer(playersScripts[i]);
                    break;
                }
            }
            if (GetMobilePlayer())
            {
                GetMobilePlayer().touchController = GetTouchController();
            }
            else
            {
                Debug.Log("problem in gameMGR: no mobile player");
            }

            UIMgr = mobileUIMgr;
            PCUIMgr.gameObject.SetActive(false);
            mobileCamera.SetActive(true);
            pcCamera.SetActive(false);
        }
        else
        {
            UIMgr = PCUIMgr;
            mobileUIMgr.gameObject.SetActive(false);
            mobileCamera.SetActive(false);
            pcCamera.SetActive(true);
        }
        UIMgr.gameObject.SetActive(true);

        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(true);
            playersScripts[i].init();
            playersScripts[i].playerOut.AddListener(playerDied);
        }


        GetColorChanger().gameObject.SetActive(colorChangerEnable);
        GetColorChanger().isTeams = isTeams;
        GetColorChanger().UIManager = UIMgr;

        dummyPlayer.SetActive(false);
    }

    public AudioManager AudioManagerRef
    {
        get
        {
            return audioManagerRef;
        }
    }

    public GameObject GetArenaChosen()
    {
        return arenaChosen;
    }

    public void SetArenaChosen(GameObject value)
    {
        arenaChosen = value;
        arenaOutRadius = arenaChosen.transform.Find("OuterBounds").position.magnitude;
        arenaInnerRadius = arenaChosen.transform.Find("InsideBounds").position.magnitude;
    }

    public ParticlesManager GetPM()
    {
        return PM;
    }

    public void SetPM(ParticlesManager value)
    {
        PM = value;
    }

    public ColorChangeManager GetColorChanger()
    {
        return colorChanger;
    }

    public void SetColorChanger(ColorChangeManager value)
    {
        colorChanger = value;
    }

    public SimpleTouchController GetTouchController()
    {
        return touchController;
    }

    public void SetTouchController(SimpleTouchController value)
    {
        touchController = value;
    }

    public AbstractPlayer GetMobilePlayer()
    {
        return mobilePlayer;
    }

    public void SetMobilePlayer(AbstractPlayer value)
    {
        mobilePlayer = value;
    }

    public int GetNumPlayersAlive()
    {
        return numPlayersAlive;
    }

    public void SetNumPlayersAlive(int value)
    {
        numPlayersAlive = value;
    }

    private void getGameParams()
    {
        if (gameParams.isInit)
        {
            charTypes = gameParams.characterArray;
            arena = gameParams.arena;
        }
    }

    private void initArena()
    {
        for(int i = 0; i < arenas.Length; i++)
        {
            arenas[i].SetActive(false);
        }
        switch (arena)
        {
            case ArenaTypes.CHEERIOS:
                SetArenaChosen(arenas[0]);
                break;
            case ArenaTypes.UGI:
                SetArenaChosen(arenas[1]);
                break;
            case ArenaTypes.FLAT:
                SetArenaChosen(arenas[2]);
                break;
        }
        GetArenaChosen().SetActive(true);
        if (isTilting)
        {
            tiltMgr = GetArenaChosen().GetComponent<TiltManager>();
            tiltMgr.enabled = true;
            milk.transform.position = new Vector3 (milk.transform.position.x,tiltMgr.milkStartingHight,milk.transform.position.z) ;
        }
        if (createPickups)
        {

            pickupMgr.Init(this);
        }
    }



    void Start()
    {
        GetColorChanger().ColorChanger();
        AudioManagerRef.Init(this);
        UIMgr.startCounter(startCountTime, "GO!!!!", true, startGame);
        AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Countdown);
        GetPM().Play_Effect(ParticlesManager.ParticleTypes.Snow,Vector3.up*SNOW_HEIGHT);
    }


private void startGame()
    {
        for (int i = 0; i < players.Length; i++)
        {
            StartCoroutine(playersScripts[i].setCanMove(true,0));
        }
        AudioManagerRef.Play_Sound(AudioManager.SoundTypes.BG_Music,true);
        // StartCoroutine(MusicUtil.FadeIn(audioSource, 3));
        if (isSuddenDeath)
        {
            InvokeRepeating("milkRiseStart", tiltMgr.MILK_RISE_TIME, tiltMgr.MILK_RISE_PERIOD);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isMilkRising)
        {
            float step = tiltMgr.MILK_RISE_SPEED * Time.deltaTime; // calculate distance to move
            milk.transform.position = Vector3.MoveTowards(milk.transform.position, milkTarget, step);
            if (Vector3.Distance(milk.transform.position, milkTarget) < 0.001f)
            {
                isMilkRising = false;
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
        if (milkRiseCount < tiltMgr.MILK_RISE_REPEATED_TIMES)
        {
            milkRiseCount++;
            isMilkRising = true;
            milkTarget = milk.transform.position + Vector3.up * tiltMgr.MILK_RISE_HIGHT;
            AudioManagerRef.AddPitch(AudioManager.SoundTypes.BG_Music, 0.1f);
            //print("milkTarget" + milkTarget);
            //print("milk");
        }
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
        GetMobilePlayer().OnFire();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(0);
    }
}
