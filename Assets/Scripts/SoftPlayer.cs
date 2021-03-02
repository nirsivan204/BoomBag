using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
 
public class SoftPlayer : AbstractPlayer {
    bool canUseAbility = true;

    public void Start()
    {
        playerIndex = 0;
    }

    protected void endAbility()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i != playerIndex)
            {
                Physics.IgnoreCollision(players[playerIndex].GetComponent<Collider>(), players[i].GetComponent<Collider>(),false);
            }
        }
        canUseAbility = true;


    }
    protected override void useAbility()
    {
        if (canUseAbility)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i != playerIndex)
                {
                    Physics.IgnoreCollision(players[playerIndex].GetComponent<Collider>(), players[i].GetComponent<Collider>());
                }
            }

            Invoke("endAbility", 5f);
            canUseAbility = false;
        }
    }
}