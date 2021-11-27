using System;
using UnityEngine;
using System.Collections;

public class PlayerIndicator : MonoBehaviour {
    //private float degreesPerSecond = 15.0f;
    [SerializeField] private float amplitude = 0.5f;
    private float frequency = 1f;
    AbstractPlayer parentPlayer;
    MeshRenderer mesh;
    Vector3 tempPos;// = new Vector3 ();
    bool isInit = false;

    // Use this for initialization
    public void init () {
        parentPlayer = transform.parent.GetComponent<AbstractPlayer>();
        if (parentPlayer &&!parentPlayer.getIsHuman())
        {
            Destroy(gameObject);
        }
        mesh = GetComponent<MeshRenderer>();
        amplitude /= 50;
        tempPos = transform.localPosition;
        isInit = true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (isInit)
        {
            // Spin object around Y-Axis
            // transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            transform.localPosition = tempPos + new Vector3(0, 2.2f, 0);


            // Fill the indicator according to player's energy:
            // mesh.material.SetTextureOffset("_MainTex" , new Vector2(0, parentPlayer.energy / (AbstractPlayer.MAX_ENERGY * 2)));
        }
    }

    private void LateUpdate()
    {
        if (isInit)
        {
            // Move to camera-player axis:
            Vector3 plyaerToIndicator = transform.position - parentPlayer.transform.position;
            Vector3 playerToCamera = Camera.main.transform.position - parentPlayer.transform.position;
            transform.position += -plyaerToIndicator + (plyaerToIndicator.magnitude / playerToCamera.magnitude) * playerToCamera;

            // Look at Camera
            transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);
        }
    }
}