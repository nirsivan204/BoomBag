using UnityEngine;

public class ColorChangeManager : MonoBehaviour
{
    private Color[] meshColors;
    public GameObject[] players;
    private AbstractPlayer[] PlayersScripts;
    public float time;
    public float repeatingTime;
    
    public void Start()
    {
        // Set Hex colors to Color name in array
        ColorUtility.TryParseHtmlString("#CDD764", out Color yellow); 
        ColorUtility.TryParseHtmlString("#E63430", out Color red);
        ColorUtility.TryParseHtmlString("#61C2C8", out Color cyan);
        ColorUtility.TryParseHtmlString("#EDA853", out Color orange);
        ColorUtility.TryParseHtmlString("#00A650", out Color green);
        ColorUtility.TryParseHtmlString("#81398F", out Color purple);
        meshColors = new Color[] {yellow, red, cyan, orange, green, purple};
        PlayersScripts = new AbstractPlayer[players.Length];
        
        for (int i = 0; i < players.Length; i++)
        {
            PlayersScripts[i] = players[i].GetComponent<AbstractPlayer>();
        }

        InvokeRepeating("ColorChanger", time, repeatingTime);
    }

    public void ColorChanger()
    {
        foreach (AbstractPlayer script in PlayersScripts)
        {
            //playerMesh.material.color = meshColors[Random.Range(0, meshColors.Length)];
            script.setColor(meshColors[Random.Range(0, meshColors.Length)]);
        }
    }
}
