using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using System;

public class VictoryScreenScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private TMP_Text winTitle;

    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text timeField;
    
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text scoreField;
    
    [SerializeField]
    private TMP_Text ratingText;
    [SerializeField]
    private TMP_Text ratingField;

    [SerializeField]
    private TMP_Text mainMenuText;
    [SerializeField]
    private TMP_Text runAgainText;
    [SerializeField]
    private TMP_Text quitText;

    
    //Counting up score visually in end screen
    private float currentScore;
    private float targetScore;

    private float currentTime;
    private float targetTime;

    void Start()
    {


        currentScore = 0;
        targetScore = GameManager.instance.getScore();


        currentTime = 0;
        targetTime = Time.time;
        scoreField.text = "0";
        ratingField.text = "0";
        scoreField.text = "0";
        DecicdeFinalRank();
        StartCoroutine(PrintEndScreenText());
    }

    
    private void DecicdeFinalRank()
    {
        //Decide final rank
        //targetTime, targetScore
        ratingField.text = "S+ (wip)";
    }
    IEnumerator PrintEndScreenText()
    {

        yield return new WaitForSeconds(2);
        winTitle.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        timeText.gameObject.SetActive(true);
        timeField.gameObject.SetActive(true);
        yield return (CountUpTime());
        yield return new WaitForSeconds(1);
        scoreText.gameObject.SetActive(true);
        scoreField.gameObject.SetActive(true);
        yield return(CountUpScore());
        yield return new WaitForSeconds(1);
        ratingText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        ratingField.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        runAgainText.gameObject.SetActive(true);
        mainMenuText.gameObject.SetActive(true);

    }

  
    IEnumerator CountUpScore()
    {
        while (currentScore < targetScore)
        {
            currentScore += 1; // or whatever to get the speed you like
            currentScore = Mathf.Clamp(currentScore, 0f, targetScore);
            scoreField.text = currentScore + "";
            yield return null;
        }
    }

    IEnumerator CountUpTime()
    {
        while (currentTime < targetTime)
        {
            currentTime += 1; // or whatever to get the speed you like
            currentTime = Mathf.Clamp(currentTime, 0f, targetTime);

            timeField.text = TimeSpan.FromSeconds(currentTime).Hours.ToString("00") + ":" +TimeSpan.FromSeconds(currentTime).Minutes.ToString("00") + ":"+ TimeSpan.FromSeconds(currentTime).Seconds.ToString("00");
            yield return null;
        }
    }
    public void RestartGame()
    {
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);
        if (TutorialManager.Instance != null) Destroy(TutorialManager.Instance.gameObject);
        SceneManager.LoadScene("Tutorial");
    }
    public void MainMenu()
    {
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);
        if (TutorialManager.Instance != null) Destroy(TutorialManager.Instance.gameObject);
        SceneManager.LoadScene("MainMenu");

    }

}
