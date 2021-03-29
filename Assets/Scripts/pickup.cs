using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        doEffect(other.GetComponent<AbstractPlayer>());
        Destroy(this.gameObject);
    }

    void doEffect(AbstractPlayer player)
    {
        print(player + " pickup");
    }
}
