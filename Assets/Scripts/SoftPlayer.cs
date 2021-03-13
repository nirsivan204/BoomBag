using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class SoftPlayer : AbstractPlayer {
    bool canUseAbility = true;
    private Collider[] playersColliders;


    protected override void init()
    {
        playersColliders = new Collider[gameManager.players.Length];
        for (int i =0; i < gameManager.players.Length; i++)
        {
            playersColliders[i] = gameManager.players[i].GetComponent<Collider>();
            if (!playersColliders[i])
            {
                print("ERROR in softPlayer");
            }
        }
    }

    protected void endAbility()
    {
        for (int i = 0; i < gameManager.players.Length; i++)
        {
            if (i != playerIndex)
            {
                Physics.IgnoreCollision(playersColliders[playerIndex], playersColliders[i], false);
            }
        }
        canUseAbility = true;


    }
    protected override void useAbility()
    {
        if (canUseAbility)
        {
            for (int i = 0; i < gameManager.players.Length; i++)
            {
                if (i != playerIndex)
                {
                    Physics.IgnoreCollision(playersColliders[playerIndex], playersColliders[i]);
                }
            }

            Invoke("endAbility", 5f);
            canUseAbility = false;
            print("GHOST");
        }
    }
}