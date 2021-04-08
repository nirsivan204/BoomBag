using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public Pickups.pickupsTypes type;
    public const float INVERT_TIME = 5;
    public const float EXPLOSION_FORCE = 40000;

    public GameObject explosionEffect;
    public float explosionRadius;

    private void OnTriggerEnter(Collider other)
    {
        doEffect(other.GetComponent<AbstractPlayer>());
        Destroy(this.gameObject);
    }

    void doEffect(AbstractPlayer player)
    {
        print(player + " pickup "+ type);
        switch (type)
        {
            case Pickups.pickupsTypes.ENLARGE:
                player.grow(5);
                Destroy(this.gameObject);
                break;
            case Pickups.pickupsTypes.SHRINK:
                player.shrink(5);
                Destroy(this.gameObject);
                break;
            case Pickups.pickupsTypes.FULLPOWER:
                player.setEnergy(AbstractPlayer.MAX_ENERGY);
                Destroy(this.gameObject);
                break;
            case Pickups.pickupsTypes.INVERTER:
                player.invertControls(INVERT_TIME);
                Destroy(this.gameObject);
                break;
            case Pickups.pickupsTypes.BOMB:
                explode();
                break;
        }
    }
    private void explode()
    {
        //Instantiate(explosionEffect);
        Collider[] coliders = Physics.OverlapSphere(transform.position, explosionRadius);
        print(coliders.Length);
        foreach (Collider player in coliders)
        {
            print(player);
            if(player.tag == "Player")
            {
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.AddExplosionForce(EXPLOSION_FORCE, transform.position, explosionRadius);
            }
        }
    }
}
