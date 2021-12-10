using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterCube : MonoBehaviour
{
    private GameManager.CharTypes currentChar = 0;
    private bool isCharChosen = false;
    [SerializeField] int playerIndex;
    private Transform target;
    private float speed = 5;
    public UnityEvent characterSelectedEvent;
    public UnityEvent characterDeSelectedEvent;
    [SerializeField] AudioSource AS;
    [SerializeField] AudioClip selectSound;
    [SerializeField] AudioClip deselectSound;
    [SerializeField] AudioClip swoosh;

    public void init()
    {
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        float movementX = movementVector.x;
        if (!isCharChosen && canMove == 0)
        {
            if (movementX < -0.3f)
            {
                turnLeft();
                canMove = -1;
            }
            else
            {
                if (movementX > 0.3f)
                {
                    turnRight();
                    canMove = 1;
                }
            }
        }
        if(canMove == 1 && movementX<=0)
        {
            canMove = 0;
        }
        if (canMove == -1 && movementX >= 0)
        {
            canMove = 0;
        }
    }

    internal void disableInput()
    {
        canMove = -2;//never move again
    }

    private void turnRight()
    {
        AS.clip = swoosh;
        AS.Play();
        currentChar = (GameManager.CharTypes)(((int)currentChar - 1) % 4);
        if((int)currentChar == -1) // workaround because c# is defining modulu like idiot
        {
            currentChar = (GameManager.CharTypes)3;
        }
        newRotation = Quaternion.Euler(0, (int)currentChar * 90, 0);
        isRotating = true;
    }
    Quaternion newRotation;
    private bool isRotating;
    private int canMove = 0;

    private void turnLeft()
    {
        AS.clip = swoosh;
        AS.Play();
        currentChar = (GameManager.CharTypes) (((int)currentChar + 1) % 4);
        newRotation = Quaternion.Euler(0, (int)currentChar*90, 0);
        isRotating = true;
    }
    public void OnFire()
    {
        if (canMove != -2)
        {
            if (!isCharChosen)
            {
                isCharChosen = true;
                gameParams.characterArray[playerIndex] = currentChar;
                characterSelectedEvent.Invoke();
                AS.clip = selectSound;
            }
            else
            {
                isCharChosen = false;
                characterDeSelectedEvent.Invoke();
                AS.clip = deselectSound;
            }
            AS.Play();

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime*speed);
            if(Quaternion.Angle(transform.rotation, newRotation) == 0)
            {
                isRotating = false;
                transform.rotation = newRotation;
            }
        }
    }
}
