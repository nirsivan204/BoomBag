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
            GameObject pickupClone = Instantiate(pickupPref, randLocation,Quaternion.identity);
            Pickup script = pickupClone.GetComponent<Pickup>();
            script.init(pickupType,GM);
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


    public static pickupsTypes getRandomType()
    {
        return (pickupsTypes)2;//(pickupsTypes)Random.Range(0, numOfTypes);
    }
}
