using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public Pickups.pickupsTypes type;
    public const float INVERT_TIME = 5;
    public const float EXPLOSION_FORCE = 40000;
    private MeshRenderer mesh;
    private Collider collider;

    public GameObject explosionEffect;
    public float explosionRadius;
    private AudioSource audioSource;
    private void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        switch (type)
        {
            case Pickups.pickupsTypes.BOMB:
                audioSource.clip = AssetsManager.AM.explosionSound;
                break;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        doEffect(other.GetComponent<AbstractPlayer>());
        vanish();
    }

    private void vanish()
    {
        mesh.enabled = false;
        collider.enabled = false;
        Invoke("destroy",1);
    }
    private void destroy()
    {
        Destroy(this.gameObject);
    }



    void doEffect(AbstractPlayer player)
    {
        print(player + " pickup "+ type);
        switch (type)
        {
            case Pickups.pickupsTypes.ENLARGE:
                player.grow(5);
                break;
            case Pickups.pickupsTypes.SHRINK:
                player.shrink(5);
                break;
            case Pickups.pickupsTypes.FULLPOWER:
                player.setEnergy(AbstractPlayer.MAX_ENERGY);
                break;
            case Pickups.pickupsTypes.INVERTER:
                player.invertControls(INVERT_TIME);
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
        audioSource.Play();
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
