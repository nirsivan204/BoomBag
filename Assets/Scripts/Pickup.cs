using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public static int numOfTypes = 5;

    public PickupsManager.pickupsTypes Type;
    public const float INVERT_TIME = 5;
    public const float EXPLOSION_FORCE = 50;
    public GameObject explosionEffect;
    private float explosionRadius = 10;
    private AudioSource audioSource;
    private bool isInit = false;

    public void init(PickupsManager.pickupsTypes type)
    {
        audioSource = GetComponent<AudioSource>();
        Type = type;
        switch (type)
        {
            case PickupsManager.pickupsTypes.BOMB:
                transform.Find("Bomb").gameObject.SetActive(true);
                break;
            case PickupsManager.pickupsTypes.ENLARGE:
                transform.Find("positive").gameObject.SetActive(true);
                break;
            case PickupsManager.pickupsTypes.SHRINK:
                transform.Find("negative").gameObject.SetActive(true);
                break;
            case PickupsManager.pickupsTypes.FULLPOWER:
                transform.Find("milk_bottle").gameObject.SetActive(true);
                break;
            case PickupsManager.pickupsTypes.INVERTER:
                transform.Find("Inverter").gameObject.SetActive(true);
                break;
        }
        isInit = true;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (isInit)
        {
            AbstractPlayer player = other.gameObject.GetComponent<AbstractPlayer>();
            if (player)
            {
                doEffect(other.gameObject.GetComponent<AbstractPlayer>());
                vanish();
            }
        }
    }

    private void vanish()
    {
        Destroy(this.gameObject);
    }

    void doEffect(AbstractPlayer player)
    {
//        print(player + " pickup "+ type);
        switch (Type)
        {
            case PickupsManager.pickupsTypes.ENLARGE:
                player.grow(5);
                break;
            case PickupsManager.pickupsTypes.SHRINK:
                player.shrink(5);
                break;
            case PickupsManager.pickupsTypes.FULLPOWER:
                player.setEnergy(AbstractPlayer.MAX_ENERGY);
                break;
            case PickupsManager.pickupsTypes.INVERTER:
                player.invertControls(INVERT_TIME);
                break;
            case PickupsManager.pickupsTypes.BOMB:
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
                rb.AddExplosionForce(EXPLOSION_FORCE*rb.mass, new Vector3(transform.position.x, player.transform.position.y,transform.position.z), explosionRadius,0.4f,ForceMode.Impulse);
                player.gameObject.GetComponent<AbstractPlayer>().disableMoveForSeconds(0.5f);
            }
        }
    }

    //must be called before activating the pickup gameobject
}
