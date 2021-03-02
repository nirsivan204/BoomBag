using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class AbstractPlayer : MonoBehaviour
{
    protected int playerIndex;
    public GameObject[] players;
    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private bool grounded = false;
    private int size = 4;
    [SerializeField] private int maxGrow = 7;
    [SerializeField] private int minGrow = 1;
    private float growRatio = 0.25f;
    public float massGrowRate = 0.2f;
    [SerializeField] private GameObject playerCharacter;
    public Color MyColor;
    private MeshRenderer playerMeshRenderer;
    private float energy;

    void Awake()
    {
        energy = 25;
        rb = GetComponent<Rigidbody>();
        MyColor = playerCharacter.GetComponent<MeshRenderer>().material.color;
        rb.freezeRotation = true;
        rb.mass = size * massGrowRate;
        playerMeshRenderer = playerCharacter.GetComponent<MeshRenderer>();
    }
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnFire()
    {
        if (energy >= 30)
        {
            useAbility();
            energy -= 30;
        }
    }

    protected virtual void useAbility()
    {

    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0, movementY);
        rb.AddForce(movement * speed, ForceMode.Acceleration);

        energy += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision otherPlayer)
    {
        AbstractPlayer other = otherPlayer.gameObject.GetComponent<AbstractPlayer>();
        if (other)
        {
            if (other.getColor() == MyColor)
            {
                grow();
            }
            else
            {
                shrink();
            }
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
}
