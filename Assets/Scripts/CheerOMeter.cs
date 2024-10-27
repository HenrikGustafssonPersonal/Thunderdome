using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CheerOMeter : MonoBehaviour
{
    public Transform pin;
    public TMP_Text scoreMultiplierText;
    public TMP_Text totalScoreText;

    
    public void SetPinRotation(float cheerScore)
    {
        //Convert current cheer score to rotation of pin;
        pin.rotation = Quaternion.Euler(0,0, (cheerScore / 1000) * -264 + 220);
       
    }
    public void SetTotalScoreText(int totalScore)
    {
        totalScoreText.text = totalScore.ToString();
    }

    public void SetScoreMultiplier(int scoreMultiplier)
    {
        scoreMultiplierText.text = scoreMultiplier.ToString() +"x";
    }
}
