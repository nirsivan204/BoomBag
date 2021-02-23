using UnityEngine;
using System.Collections;

public class PlayerIndicator : MonoBehaviour {
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public GameObject player;
   
    public float playerHeight = 2.0f;
    
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
 
    // Use this for initialization
    void Start () {
        // Store the starting position & rotation of the object
      //  posOffset = transform.position;
      transform.position = player.transform.position + new Vector3(0, 4, 0);
    }
     
    // Update is called once per frame
    void Update ()
    {
        float playerSize = player.transform.localScale.x;
        //1.75-0.25
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
 
        // Float up/down with a Sin()
      //  tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude /50;
        transform.position = tempPos + new Vector3(0, playerHeight, 0);

        transform.position += player.transform.position;

    }
}