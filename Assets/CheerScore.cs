using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheerScore : MonoBehaviour
{


    public float currentCheerScore;
    private float targetCheerScore;
    public float cheerScoreDecayRate = 10.0F;
    
    //Dont change
    private float maxCheerScore = 1000.0F;
    public int scoreMultiplier = 1;
    public int totalScore = 0;


    void Start()
    {
        
        currentCheerScore = 0;

        GameManager.instance.setCheerOMeter(currentCheerScore);

    }

    void Update()
    {
        CheerScoreDecay();

        ScoreMultiplier();

        //Update cheer gauge
        GameManager.instance.setCheerOMeter(currentCheerScore);
        GameManager.instance.setTotalScoreText(totalScore);
        GameManager.instance.setScoreMultiplier(scoreMultiplier);
        currentCheerScore = Mathf.Lerp(currentCheerScore, targetCheerScore, Time.deltaTime * 10.0f);

    }
    public void CheerScoreDecay()
    {
        if (targetCheerScore > 0)
        {
            targetCheerScore -= cheerScoreDecayRate * Time.deltaTime;

        }
    }
    public void AddToCheerScore(float amount)

    {
        Debug.Log("TRIED TO ADD CHEER SCORE");
        if (targetCheerScore + amount < maxCheerScore)
            targetCheerScore += amount;
        else
            targetCheerScore = maxCheerScore;

    }
    public void ScoreMultiplier()
    {
        
        //Ranges correlating to UI cheer o meter
        bool rangeMultiplier1 = currentCheerScore < 240;
        bool rangeMultiplier2 = currentCheerScore >= 240 && currentCheerScore < 495;
        bool rangeMultiplier3 = currentCheerScore >= 495 && currentCheerScore < 750;
        bool rangeMultiplier4 =  currentCheerScore >= 750 && currentCheerScore < 900;
        bool rangeMultiplier5 = currentCheerScore >= 900;

        //Set score multiplier depending on current cheer score

        if (rangeMultiplier1)
        {
            scoreMultiplier = 1;
        } else if (rangeMultiplier2)
        {
            scoreMultiplier = 2;

        }
        else if (rangeMultiplier3)
        {
            scoreMultiplier = 3;

        }
        else if (rangeMultiplier4)
        {
            scoreMultiplier = 4;

        }
        else if (rangeMultiplier5)
        {
            scoreMultiplier = 5;

        }

    }

    public void AddToTotalScore(int amount)
    {

        totalScore += amount * scoreMultiplier;
    }
}
