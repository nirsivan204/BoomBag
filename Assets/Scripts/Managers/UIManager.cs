using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    // bool middleCount;
    private FuncToCall funcToCall;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private List<Image> energyBars;
    [SerializeField] private Button actionButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    


    void Start()
    {
        gm.winEvent.AddListener(gameFinish);
        if (restartButton)
        {
            restartButton.onClick.AddListener(gm.restartGame);

        }
        animator = GetComponent<Animator>();

        // energyBars = new List<Image>();
        // foreach (Transform child in transform.Find("energyBars"))
		// {
        //     energyBars.Add(child.Find("energy").gameObject.GetComponent<Image>());
        // }

        if (actionButton)
        {
            actionButton.onClick.AddListener(gm.mobilePlayerPressedAction);
        }
    }

    void Update()
    {
        if (energyBars != null)
        {
            int i = 0;
            foreach (var energyBar in energyBars)
			{
                energyBar.fillAmount = gm.playersScripts[i].energy / AbstractPlayer.MAX_ENERGY;
                i++;
            }
            if (actionButton && gm.isMobileGame && gm.GetMobilePlayer())
            {
                ColorBlock colors = actionButton.colors;
                if (gm.GetMobilePlayer().energy > AbstractPlayer.ENERGY_COST)
                {

                    colors.pressedColor = Color.green;
                }
                else
                {
                    colors.pressedColor = Color.red;
                }
                actionButton.colors = colors;

            }
        }
    }

    private void gameFinish(int winnerIndex)
    {
        counterTextCorner.SetText("");
        counterTextMiddle.SetText("");
        counterTextCorner.gameObject.SetActive(false);
        setWinText(winnerIndex+1);
        StartCoroutine(deleteText(winText, 4));
        if (restartButton)
        {
            restartButton.gameObject.SetActive(true);
        }
    }

    public void setWinText(int winnerIndex)
    {
        winText.SetText("Player "+ winnerIndex + " wins!");
    }

    public void setScoreText(int [] scores)
    {
        if (scoreText)
        {
            scoreText.SetText(scores[0] + ":" + scores[1] + ":" + scores[2] + ":" + scores[3]);
        }
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

    public void showMsg(string msg, int duration, bool isMiddle)
    {
        TMP_Text text;
        if (isMiddle)
        {
            text = counterTextMiddle;
        }
        else
        {
            text = counterTextCorner;
        }
        text.SetText(msg);
        StartCoroutine(deleteText(text, duration));
    }



    private IEnumerator deleteText(TMP_Text textToDelete, float time = 1)
    {
        yield return new WaitForSeconds(time);
        textToDelete.SetText("");
    }
}