﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
 
public class SoftPlayer : AbstractPlayer {
    bool canUseAbility = true;
    private Collider[] playersColliders;
    //private AudioClip ghostSound;
    public override void init()
    {
        //ghostSound = AssetsManager.AM.ghostSound;
        base.init();
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
            showAbilityEffect();
            Invoke("endAbility", 5f);
            canUseAbility = false;
        }
    }

    protected override void showAbilityEffect()
    {
        //audioSource.clip = ghostSound;
        //audioSource.Play();
        gameManager.AudioManagerRef.Play_Sound(AudioManager.SoundTypes.Ghost , player_index: playerIndex);
        setTransparent(true);
    }
    public override void setColor(Color color)
    {
        base.setColor(color);
        if (isTransparent)
        {
            setTransparent(isTransparent);
        }
    }

    public void setTransparent(bool transparent)
    {
        Color current = GetPlayerMeshRenderer().material.color;
        Color changed = current;
        if (transparent)
        {
            GetPlayerMeshRenderer().material = gameManager.GetColorChanger().transparentMat;
            changed.a = 0.5f;
            foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
            {
                mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            }
        }
        else
        {
            GetPlayerMeshRenderer().material = gameManager.GetColorChanger().regulatMat;
            changed.a = 1;
            foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
            {
                mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
        GetPlayerMeshRenderer().material.color = changed;
        isTransparent = transparent;
    }

}