using UnityEngine;

public class ColorChangeManager : MonoBehaviour
{
    public bool isTeams = true;
    private Color[] meshColors;
    public GameManager gameManager;
    private AbstractPlayer[] PlayersScripts;
//    public int time;
    public int repeatingTime;
    public UIManager UIManager;
    public int timeToShowCount;
    [SerializeField] public Material transparentMat;
    [SerializeField] public Material regulatMat;
    private bool isFirstColorChange = true;

    public Material TransparentMat { get => transparentMat; set => transparentMat = value; }
    public Material RegulatMat { get => regulatMat; set => regulatMat = value; }

    public void Awake()
    {
        // Set Hex colors to Color name in array
        ColorUtility.TryParseHtmlString("#CDD764", out Color yellow); 
        ColorUtility.TryParseHtmlString("#E63430", out Color red);
        ColorUtility.TryParseHtmlString("#61C2C8", out Color cyan);
        ColorUtility.TryParseHtmlString("#EDA853", out Color orange);
        ColorUtility.TryParseHtmlString("#00A650", out Color green);
        ColorUtility.TryParseHtmlString("#81398F", out Color purple);
        meshColors = new Color[] {yellow, red, cyan, orange, green, purple};
        PlayersScripts = new AbstractPlayer[gameManager.players.Length];
        
        for (int i = 0; i < gameManager.players.Length; i++)
        {
            PlayersScripts[i] = gameManager.players[i].GetComponent<AbstractPlayer>();
        }
    }

    private void startCount()
    {
        Invoke("startShowCount", repeatingTime - timeToShowCount);
    }

    private void startShowCount()
    {
        UIManager.startCounter(timeToShowCount, "COLOR MIX!",false, ColorChanger);
    }

    int lastDecisionForTwoPlayers = -1;
    public void ColorChanger()
    {
        if (!isFirstColorChange)
        {
            gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.ColorChange);
        }
        else
        {
            isFirstColorChange = false;
        }
        if (isTeams)
        {
            int firstRandomPlayer; 
            int secondRandomPlayer;
            getTwoRandomPlayersIdx(out firstRandomPlayer, out secondRandomPlayer, gameManager.GetNumPlayersAlive() == 2);
            if(gameManager.GetNumPlayersAlive() == 2)
            {
                if(lastDecisionForTwoPlayers == -1)//first color change for 2 people
                {
                    repeatingTime /= 2;

                }
                int decision = Random.Range(0, 4);
                while(lastDecisionForTwoPlayers== decision)
                {
                    decision = Random.Range(0, 4);
                }
                switch (decision)
                {
                    case 0: //both red
                        PlayersScripts[firstRandomPlayer].setColor(meshColors[1]);
                        PlayersScripts[secondRandomPlayer].setColor(meshColors[1]);
                        break;
                    case 1: // both blue
                        PlayersScripts[firstRandomPlayer].setColor(meshColors[2]);
                        PlayersScripts[secondRandomPlayer].setColor(meshColors[2]);
                        break;
                    case 2: // first red second blue
                        PlayersScripts[firstRandomPlayer].setColor(meshColors[1]);
                        PlayersScripts[secondRandomPlayer].setColor(meshColors[2]);
                        break;
                    case 3: // first blue second red
                        PlayersScripts[firstRandomPlayer].setColor(meshColors[2]);
                        PlayersScripts[secondRandomPlayer].setColor(meshColors[1]);
                        break;
                }
                lastDecisionForTwoPlayers = decision;
            }
            else
            {
                for (int i = 0; i < PlayersScripts.Length; i++)
                {
                    AbstractPlayer script = PlayersScripts[i];
                    if (i == firstRandomPlayer || i == secondRandomPlayer)
                    {
                        script.setColor(meshColors[1]);

                    }
                    else
                    {
                        script.setColor(meshColors[2]);
                    }
                }
            }
        }
        else
        {
            foreach (AbstractPlayer script in PlayersScripts)
            {
                script.setColor(meshColors[Random.Range(0, meshColors.Length)]);
            }
        }
        startCount();
    }

    private void getTwoRandomPlayersIdx(out int first, out int second, bool isMustBeAlive)
    {
        first = Random.Range(0, PlayersScripts.Length);
        while(isMustBeAlive && PlayersScripts[first].getIsOut())
        {
            first = Random.Range(0, PlayersScripts.Length);
        }
        second = Random.Range(0, PlayersScripts.Length);
        while (second == first || (isMustBeAlive && PlayersScripts[second].getIsOut()))
        {
            second = Random.Range(0, PlayersScripts.Length);
        }

    }
}
