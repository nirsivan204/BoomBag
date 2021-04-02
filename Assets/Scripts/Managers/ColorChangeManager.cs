using UnityEngine;

public class ColorChangeManager : MonoBehaviour
{
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
        foreach (AbstractPlayer script in PlayersScripts)
        {
            script.setColor(meshColors[Random.Range(0, meshColors.Length)]);
        }
        startCount();
    }
}
