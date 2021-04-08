using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class AbstractPlayer : MonoBehaviour
{
    protected int playerIndex;
    protected GameManager gameManager;
    public float speed = 80.0f;
    protected Rigidbody rb;
    public float drag = 0;
    private float movementX = 0;
    private float movementY = 0;
    protected bool grounded = true;
    private int size; //= startSize in start;
    [SerializeField] private int maxGrow = 20;
    [SerializeField] private int minGrow = 1;
    private float growRatio = 1.4f;  // must be less than startSize/(startSize - minGrow)
    public float massGrowRate = 1.05f; // must be less than startSize/(startSize - minGrow)
    private int startSize = 2;
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
    public float bumpForce = 800;
    protected bool isRigid = false;
    private bool canMove = false;
    private int invertFactor = 1;
    private int numSequentialInverts = 0;

    void Awake()
    {
        playerCharacter = transform.Find("Character").gameObject;
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
        MyColor = playerCharacter.GetComponent<MeshRenderer>().material.color;
        rb.freezeRotation = true;
        playerMeshRenderer = playerCharacter.GetComponent<MeshRenderer>();
        playerOut = new IntEvent();
        size = startSize;
        transform.localScale = new Vector3(startSize, startSize, startSize);
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
        Vector2 movementVector = movementValue.Get<Vector2>();//*invertFactor;
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
            return;
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
        if (canMove)
        {
            Vector3 movement = new Vector3(movementX, 0, movementY);
            rb.AddForce(movement * speed * rb.mass * invertFactor);
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
            if (!isRigid)
            {
                rb.AddExplosionForce(bumpForce * other.getMass(), (other.transform.position + transform.position) / 2, 100, 0);//, ForceMode.Acceleration); or other.size??maybe cancel mass at all??
            }
            else
            {
                Vector3 collisionForce = otherPlayer.impulse;
                rb.AddForce(collisionForce, ForceMode.Impulse);
            }
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
            else if (otherPlayer.gameObject.tag == "Out" && !isOut)
            {
                print(gameObject + "out, player index = " + playerIndex);
                playerOut.Invoke(playerIndex);
                isOut = true;
                canMove = false;
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
        public void grow(int times = 1)
    {
        if (size < maxGrow)
        {
            size = Math.Min(maxGrow,size + times);
            setNewSize();
        }
    }

    public void shrink(int times = 1)
    {
        if (size > minGrow)
        {
            size = Math.Max(minGrow, size - times);
            setNewSize();
        }
    }

    private void setNewSize()
    {
        //float newSize = size * growRatio * sizeNormalizer;
        float multiplyCoefficient = (float)size / startSize - 1;
        float newSize = startSize + growRatio * multiplyCoefficient;
        transform.localScale = new Vector3(newSize, newSize, newSize);
        rb.mass = 1 + massGrowRate * multiplyCoefficient;
        print("mass " + rb.mass);

    }

    public void invertControls(float time)
    {
    
        if (invertFactor == -1)
        {
            numSequentialInverts++;
        }
        invertFactor *= -1;
        Invoke("unInvertControls", time);
    }

    public void unInvertControls()
    {
        if (numSequentialInverts == 0)
        {
            invertFactor = 1;
        }
        numSequentialInverts--;
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

    public void setCanMove(bool Move)
    {
        canMove = Move;
    }

    public void setEnergy(float energy)
    {
        this.energy = energy;
    }

}
