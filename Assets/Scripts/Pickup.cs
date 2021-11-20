using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public static int numOfTypes = 5;

    public PickupsManager.pickupsTypes Type;
    public const float INVERT_DURATION = 5;
    public const float EXPLOSION_FORCE = 50;
    public GameObject explosionEffect;
    private float explosionRadius = 10;
    private AudioSource audioSource;
    private bool isInit = false;
    GameManager GM;

    public void init(PickupsManager.pickupsTypes type, GameManager gm)
    {
        GM = gm;
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
                GM.GetPM().Play_Effect(ParticlesManager.ParticleTypes.Enlarge, transform.position);
                GM.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Pickup_Enlarge);
                player.grow(5,true);
                break;
            case PickupsManager.pickupsTypes.SHRINK:
                GM.GetPM().Play_Effect(ParticlesManager.ParticleTypes.Shrink, transform.position);
                GM.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Pickup_Shrink);
                player.shrink(5, true);
                break;
            case PickupsManager.pickupsTypes.FULLPOWER:
                GM.GetPM().Play_Effect(ParticlesManager.ParticleTypes.Mana, transform.position);
                GM.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Pickup_Mana);
                player.setEnergy(AbstractPlayer.MAX_ENERGY);
                break;
            case PickupsManager.pickupsTypes.INVERTER:
                GM.GetPM().Play_Effect(ParticlesManager.ParticleTypes.Inverser, transform.position);
                GM.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Pickup_Inverter);
                player.invertControls(INVERT_DURATION);
                player.GetPlayerMeshRenderer().material.color = Color.green;
                break;
            case PickupsManager.pickupsTypes.BOMB:
                GM.GetPM().Play_Effect(ParticlesManager.ParticleTypes.Boom, transform.position);
                GM.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Boom);
                explode();
                break;
        }
    }
    private void explode()
    {
        //Instantiate(explosionEffect);
        Collider[] coliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider player in coliders)
        {
            if(player.tag == "Player")
            {
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.AddExplosionForce(EXPLOSION_FORCE*rb.mass, new Vector3(transform.position.x, player.transform.position.y,transform.position.z), explosionRadius,0.4f,ForceMode.Impulse);
                player.gameObject.GetComponent<AbstractPlayer>().disableMoveForSeconds(0.5f);
            }
        }
    }
}
