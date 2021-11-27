using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMGR : MonoBehaviour
{
    [SerializeField] public PlayerInputManager PIM;
    // Start is called before the first frame update

    public void OnPlayerJoined(InputAction IA)
    {
        Debug.Log("join");
    }
}
