using UnityEngine;

public class ColorChangeManager : MonoBehaviour
{
    private Color[] meshColors;
    public GameObject[] players;
    private MeshRenderer[] meshRenderers;
    
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
        meshRenderers = new MeshRenderer[players.Length];
        
        for (int i = 0; i < players.Length; i++)
        {
            meshRenderers[i] = players[i].GetComponent<MeshRenderer>();
        }

        InvokeRepeating("ColorChanger", 3, 3);
    }

    public void ColorChanger()
    {
        foreach (MeshRenderer playerMesh in meshRenderers)
        {
            playerMesh.material.color = meshColors[Random.Range(0, meshColors.Length)];
        }
    }
}
