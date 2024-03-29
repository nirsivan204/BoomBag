﻿using UnityEngine;
using System.Collections;

public class PlayerIndicator : MonoBehaviour {
    private float degreesPerSecond = 15.0f;
    private float amplitude = 0.5f;
    private float frequency = 1f;
    AbstractPlayer parentPlayer;
    MeshRenderer mesh;
    Vector3 tempPos;// = new Vector3 ();
 
    // Use this for initialization
    void Start () {
        parentPlayer = transform.parent.GetComponent<AbstractPlayer>();
        mesh = GetComponent<MeshRenderer>();
        amplitude /= 50;
    }

    // Update is called once per frame
    void Update ()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
 
        // Float up/down with a Sin()
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.localPosition = tempPos + new Vector3(0, 2.2f , 0);
        
        // Fill the indicator according to player's energy:
        mesh.material.SetTextureOffset("_MainTex" , new Vector2(0, parentPlayer.energy / (AbstractPlayer.MAX_ENERGY * 2)));
    }
}