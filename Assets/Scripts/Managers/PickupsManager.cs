using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupsManager : MonoBehaviour
{
    private float xBoundery;
    private float yBoundery;
    private float zBoundery;

    GameObject pickupBounderies;
    GameObject arena;
    [SerializeField] float startTime;
    [SerializeField]
    public enum pickupsTypes { ENLARGE, SHRINK, INVERTER, FULLPOWER, BOMB }
    private static int numOfTypes = 5;
    [SerializeField] GameObject pickupPref;
    GameManager GM;
    [SerializeField] float repeatRate;
    bool isCreating = false;


    public void stopCreating()
    {
        isCreating = false;
    }
    public void Init(GameManager gameManager)
    {
        GM = gameManager;
        arena = gameManager.GetArenaChosen();
        pickupBounderies = arena.transform.Find("pickupsBounderies").gameObject;
        if (!pickupBounderies)
        {
            print("error in finding pickup bounderies");
        }
        calculateBounderiesOfPickups();
        isCreating = true;
        InvokeRepeating("createPickup", startTime, repeatRate);
    }

    private void createPickup()
    {
        if (isCreating)
        {
            pickupsTypes pickupType = getRandomType();
            Vector3 randLocation = getRandomLocation();
            GameObject pickupClone = Instantiate(pickupPref, arena.transform);
            pickupClone.transform.position = randLocation;

            Vector3 scaleTmp = pickupClone.transform.localScale;


            scaleTmp.x /= arena.transform.localScale.x;
            scaleTmp.y /= arena.transform.localScale.z;
            scaleTmp.z /= arena.transform.localScale.y;
            pickupClone.transform.localScale = scaleTmp;
            //pickupClone.transform.localRotation = Quaternion.Euler(90, 0, 0);

            Pickup script = pickupClone.GetComponent<Pickup>();
            script.init(pickupType);
            pickupClone.SetActive(true);
        }
    }

    private void calculateBounderiesOfPickups()
    {


        //   return center + new Vector3(
        //       (Random.value - 0.5f) * pickupBounderies.transform.localScale.x,
        //       pickupBounderies.transform.position.y,
        //        (Random.value - 0.5f) * pickupBounderies.transform.localScale.z)
        //);
        xBoundery = pickupBounderies.transform.lossyScale.x;
        yBoundery = pickupBounderies.transform.position.y;
        zBoundery = pickupBounderies.transform.lossyScale.z;
    }

    private Vector3 getRandomLocation()
    {
        return new Vector3((Random.value - 0.5f) * xBoundery, yBoundery, (Random.value - 0.5f) * zBoundery);
    }

/*
    private GameObject getParticleSystemRef(ParticleTypes particleType)
    {
        for (int i = 0; i < ParticleType_And_Ref_List.Count; i++)
        {
            if (ParticleType_And_Ref_List[i].ParticleType == particleType)
                return ParticleType_And_Ref_List[i].particleRef;
        }

        return null;
    }*/

    public static pickupsTypes getRandomType()
    {
        return (pickupsTypes)Random.Range(0, numOfTypes);
    }
}
