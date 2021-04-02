using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This does not work currently - however it shows how bad I am at coding, which is remarkable on it's own.

public class UIManager : MonoBehaviour
{
    public TMP_Text winText;
    public GameObject baseGround;
    public GameManager gm;
    public TMP_Text counterTextMiddle;
    public TMP_Text counterTextCorner;
    private TMP_Text currentCounter;
    private int counter;
    private Animator animator;
    string counterMsg;
    public delegate void FuncToCall();
    bool middleCount;
    private FuncToCall funcToCall;


    void Start()
    {
        gm.winEvent.AddListener(setWinText);
        animator = GetComponent<Animator>();
        //startCounter(5, "GO!!");
    }

    void Update()
    {

    }

    public void setWinText(int winnerIndex)
    {

        winText.SetText("Player "+ winnerIndex + " wins!");
    }

    public void startCounter(int duration, string msg, bool isMiddle, FuncToCall func)
    {
        counter = duration;
        counterMsg = msg;
        funcToCall = func;
        if (isMiddle)
        {
            currentCounter = counterTextMiddle;
        }
        else
        {
            currentCounter = counterTextCorner;
        }
        count();
    }

    public void count()
    {
        if(counter > 0)
        {
            Invoke("count", 1);
            currentCounter.SetText(counter.ToString());
            counter--;
        }
        else
        {
            currentCounter.SetText(counterMsg);
            StartCoroutine(deleteText(currentCounter));
            if (funcToCall!=null)
            {
                funcToCall();
            }
        }
    }


    private IEnumerator deleteText(TMP_Text textToDelete)
    {
        yield return new WaitForSeconds(1);
        textToDelete.SetText("");
    }
}