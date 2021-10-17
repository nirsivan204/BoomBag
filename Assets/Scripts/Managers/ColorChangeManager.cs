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
        startCount();
    }

    private void startCount()
    {
        Invoke("startShowCount", repeatingTime - timeToShowCount);
    }

    private void startShowCount()
    {
        UIManager.startCounter(timeToShowCount, "COLOR MIX!",false, ColorChanger);
    }


    public void ColorChanger()
    {
        if (isTeams)
        {
            int firstRandomPlayer = Random.Range(0, PlayersScripts.Length);
            int secondRandomPlayer = firstRandomPlayer;
            while (secondRandomPlayer== firstRandomPlayer)
            {
                secondRandomPlayer = Random.Range(0, PlayersScripts.Length);
            }
            for (int i=0; i < PlayersScripts.Length; i++ )
            {
                AbstractPlayer script = PlayersScripts[i];
                if(i == firstRandomPlayer|| i == secondRandomPlayer)
                {
                    script.setColor(meshColors[1]);

                }
                else
                {
                    script.setColor(meshColors[2]);
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
}
