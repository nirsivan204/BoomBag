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
    public void init()
    {
        characterSelectedEvent = new UnityEvent();
    }

    private void OnMove(InputValue movementValue)
    {
        if (!isCharChosen)
        {
            Vector2 movementVector = movementValue.Get<Vector2>();
            float movementX = movementVector.x;
            if (movementVector.x < -0.3f)
            {
                turnLeft();
            }
            if (movementVector.x > 0.3f)
            {
                turnRight();
            }
        }
    }

    private void turnRight()
    {
        currentChar = (GameManager.CharTypes)(((int)currentChar - 1) % 4);
        if((int)currentChar == -1) // workaround because c# is defining modulu like idiot
        {
            currentChar = (GameManager.CharTypes)3;
        }
        newRotation = Quaternion.Euler(0, (int)currentChar * 90, 0);
        isRotating = true;
    }
    Quaternion oldRotation;
    Quaternion newRotation;
    private bool isRotating;

    private void turnLeft()
    {
        currentChar = (GameManager.CharTypes) (((int)currentChar + 1) % 4);
        newRotation = Quaternion.Euler(0, (int)currentChar*90, 0);
        isRotating = true;

    }
    public void OnFire()
    {
        if (!isCharChosen)
        {
            isCharChosen = true;
            gameParams.characterArray[playerIndex] = currentChar;
            characterSelectedEvent.Invoke();
            print(currentChar);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime*speed);
            if(transform.rotation == newRotation)
            {
                isRotating = false;
                transform.rotation = newRotation;
            }
        }
    }
}
