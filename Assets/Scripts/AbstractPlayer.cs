﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class AbstractPlayer : MonoBehaviour
{
    protected int playerIndex;
    protected GameManager gameManager;
    public float speed = 30.0f;
    protected Rigidbody rb;
    private float movementX = 0;
    private float movementY = 0;
    protected bool grounded = true;
    private int size = 6;
    [SerializeField] private int maxGrow = 10;
    [SerializeField] private int minGrow = 4;
    private float growRatio = 0.25f;
    public float massGrowRate = 0.2f;
    private GameObject playerCharacter;
    public Color MyColor;
    private MeshRenderer playerMeshRenderer;
    // Energy units are equivalent to seconds:
    public float energy = 4;
    public const float MAX_ENERGY = 15;
    private const float ENERGY_COST = 10;
    public IntEvent playerOut;
    protected bool isOut = false;
    bool isHumanPlayer = true;
    private GameObject AITargetGameObj;
    private AbstractPlayer AITargetScript;
    public float maxSpeed = 20;
    public bool overSpeedAllowed = false;
    public float bumpForce = 450;
    void Awake()
    {
        playerCharacter = transform.Find("Character").gameObject;
        rb = GetComponent<Rigidbody>();
        MyColor = playerCharacter.GetComponent<MeshRenderer>().material.color;
        rb.freezeRotation = true;
        rb.mass = size * massGrowRate;
        playerMeshRenderer = playerCharacter.GetComponent<MeshRenderer>();
        playerOut = new IntEvent();
        if (!isHumanPlayer)
        {
            GetComponent<PlayerInput>().enabled = false;
            chooseTarget();
            Invoke("aiCalculateMove", 0.5f);
        }
        init();
    }


    protected virtual void init()
    {

    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnFire()
    {
        if (energy >= ENERGY_COST)
        {
            energy -= ENERGY_COST;
            useAbility();
        }
    }

    protected virtual void useAbility()
    {

    }

    private void chooseTarget()
    {
        int res = playerIndex;
        int count = 0;
        if (isOut)
        {
            AITargetGameObj = gameObject;
            AITargetScript = this;
        }
        while (res == playerIndex || !gameManager.liveOrDead[res])
        {
            res = UnityEngine.Random.Range(0, gameManager.players.Length);
            count++;
            if(count > 100)
            {
                print("ERROR in chooseTarget in " + gameObject);
                break;
            }
        }
        AITargetGameObj = gameManager.players[res];
        AITargetScript = gameManager.playersScripts[res];
        Invoke("chooseTarget", 5);


    }
    private void aiCalculateMove()
    {
        if (AITargetScript.isOut)
        {
            chooseTarget();
        }
        Vector3 attackLine = (AITargetGameObj.transform.position - transform.position);
        attackLine = new Vector3(attackLine.x, 0, attackLine.z);
        if (attackLine.magnitude <= 0)
        {
            movementX = 0;
            movementY = 0;
        }
        else
        {
            movementX = attackLine.x / attackLine.magnitude;
            movementY = attackLine.z / attackLine.magnitude;
        }
        if (!isOut)
        {
            Invoke("aiCalculateMove", 0.1f);
        }
    }

    void FixedUpdate()
    {
        if (!isOut)
        {
            Vector3 movement = new Vector3(movementX, 0, movementY);
            rb.AddForce(movement * speed * rb.mass);
            if (!overSpeedAllowed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
            if (energy < MAX_ENERGY)
            {
                energy += Time.deltaTime;
            }
        }


    }

    private void OnCollisionEnter(Collision otherPlayer)
    {
        AbstractPlayer other = otherPlayer.gameObject.GetComponent<AbstractPlayer>();
        if (other)
        {
            rb.constraints |= RigidbodyConstraints.FreezePositionY;
            rb.AddExplosionForce(bumpForce * other.getMass(), (other.transform.position + transform.position) / 2, 100, 0);//, ForceMode.Acceleration); or other.size??maybe cancel mass at all??
            if (other.getColor() == MyColor)
            {
                grow();
            }
            else
            {
                shrink();
            }
        }
        else
        {
            if(otherPlayer.gameObject.tag == "Arena")
            {
                grounded = true;
            }
            else if (otherPlayer.gameObject.tag == "Out")
            {
                print(gameObject + "out, player index = " + playerIndex);
                playerOut.Invoke(playerIndex);
                isOut = true;
            }
        }
    }

    private void OnCollisionExit(Collision otherPlayer)
    {
        AbstractPlayer other = otherPlayer.gameObject.GetComponent<AbstractPlayer>();
        if (other)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }
        public void grow()
    {
        if (size < maxGrow)
        {
            size++;
            float newSize = size * growRatio;
            transform.localScale = new Vector3(newSize, newSize, newSize);
            rb.mass = size * massGrowRate;
        }
    }

    public void shrink()
    {
        if (size > minGrow)
        {
            size--;
            float newSize = size * growRatio;
            transform.localScale = new Vector3(newSize, newSize, newSize);
            rb.mass = size * massGrowRate;
        }
    }

    public void setColor(Color color)
    {
        MyColor = color;
        playerMeshRenderer.material.color = color;
    }

    public Color getColor()
    {
        return MyColor;
    }

    public int getIndex()
    {
        return playerIndex;
    }

    public void setPlayerIndex(int index)
    {
        playerIndex = index;
    }

    public void setGameManager(GameManager gm)
    {
        gameManager = gm;
    }

    public bool getIsOut()
    {
        return isOut;
    }

    public bool isInFrame(float mergin)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return (screenPoint.z > mergin && screenPoint.x > mergin && screenPoint.x < 1- mergin && screenPoint.y > mergin && screenPoint.y < 1- mergin);
    }

    public void setIsHuman(bool isHuman)
    {
        isHumanPlayer = isHuman;
    }

    public bool getIsHuman()
    {
        return isHumanPlayer;
    }

    public float getMass()
    {
        return rb.mass;
    }
}
