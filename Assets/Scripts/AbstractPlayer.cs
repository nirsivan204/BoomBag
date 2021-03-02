using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

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
    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.mass = size * massGrowRate;
    }
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnFire()
    {
        useAbility();
    }

    protected virtual void useAbility()
    {
        
    }    
    void FixedUpdate ()
    {
        Vector3 movement = new Vector3(movementX, 0, movementY);
        rb.AddForce(movement * speed);
        //if (movementY > 0)
        //{
        //    grow();
       // }
       // if (movementY < 0)
       // {
       //     shrink();
       // }

    }

    void OnCollisionStay () {
        grounded = true;    
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
}
    