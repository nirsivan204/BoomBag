using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (BoxCollider))]
 
public class SoftPlayer : AbstractPlayer {
    bool canUseAbility = true;
    private Collider[] playersColliders;
    private AudioSource audioSource;
    private bool isTransparent = false;

    protected override void init()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = AssetsManager.AM.ghostSound;
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

        setTransparent(false);
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
            audioSource.Play();
            setTransparent(true);
            Invoke("endAbility", 5f);
            canUseAbility = false;
            print("GHOST");
        }
    }

    public override void setColor(Color color)
    {
        base.setColor(color);
        if (isTransparent)
        {
            setTransparent(isTransparent);
        }
    }

    private void setTransparent(bool transparent)
    {
        Color c = playerMeshRenderer.material.color;
        if (transparent)
        {
            c.a = 0.5f;
        }
        else
        {
            c.a = 1;
        }
        playerMeshRenderer.material.color = c;
        isTransparent = transparent;
    }

}