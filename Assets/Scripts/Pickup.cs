using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum pickupsTypes { ENLARGE, SHRINK, INVERTER, FULLPOWER, BOMB }
    public static int numOfTypes = 5;

    public pickupsTypes type;
    public const float INVERT_TIME = 5;
    public const float EXPLOSION_FORCE = 5000;
    private Collider col;
    public GameObject explosionEffect;
    public float explosionRadius;
    private AudioSource audioSource;
    private bool isTypeSet = false; 
    private void Start()
    {
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        if (!isTypeSet)
        {
            print("warning: type not set!");
        }
        switch (type)
        {
            case pickupsTypes.BOMB:
                audioSource.clip = AssetsManager.AM.explosionSound;
                transform.Find("Bomb").gameObject.SetActive(true);
                break;
            case pickupsTypes.ENLARGE:
                transform.Find("positive").gameObject.SetActive(true);
                break;
            case pickupsTypes.SHRINK:
                transform.Find("negative").gameObject.SetActive(true);
                break;
            case pickupsTypes.FULLPOWER:
                transform.Find("milk_bottle").gameObject.SetActive(true);
                break;
            case pickupsTypes.INVERTER:
                transform.Find("Inverter").gameObject.SetActive(true);
                break;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        AbstractPlayer player = other.GetComponent<AbstractPlayer>();
        if (player)
        {
            doEffect(other.GetComponent<AbstractPlayer>());
            vanish();
        }
    }

    private void vanish()
    {
        Destroy(this.gameObject);
    }

    void doEffect(AbstractPlayer player)
    {
        print(player + " pickup "+ type);
        switch (type)
        {
            case pickupsTypes.ENLARGE:
                player.grow(5);
                break;
            case pickupsTypes.SHRINK:
                player.shrink(5);
                break;
            case pickupsTypes.FULLPOWER:
                player.setEnergy(AbstractPlayer.MAX_ENERGY);
                break;
            case pickupsTypes.INVERTER:
                player.invertControls(INVERT_TIME);
                break;
            case pickupsTypes.BOMB:
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

    //must be called before activating the pickup gameobject
    public void setType(pickupsTypes typeToSet)
    {
        type = typeToSet;
        isTypeSet = true;
    }

    public static pickupsTypes getRandomType()
    {
        return (pickupsTypes)UnityEngine.Random.Range(0, numOfTypes);
    }
}
