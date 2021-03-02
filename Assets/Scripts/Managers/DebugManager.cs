using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    private void OnGrow1()
    {
        player1.GetComponent<AbstractPlayer>().grow(); ;
    }

    private void OnGrow2()
    {
        player2.GetComponent<AbstractPlayer>().grow(); ;
    }

    private void OnShrink1()
    {
        player1.GetComponent<AbstractPlayer>().shrink(); ;
    }

    private void OnShrink2()
    {
        player2.GetComponent<AbstractPlayer>().shrink(); ;
    }

    private void OnResetScene()
    {
        //Resets the Scene on button press
        SceneManager.LoadSceneAsync("InitialTestScene");
    }

    
}
