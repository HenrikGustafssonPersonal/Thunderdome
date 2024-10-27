using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;
    private float currentAlpha = 0.0f;
    private bool sceneSwapInProgress = false;

    [SerializeField]
    private OptionsMenu OP;

    private void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0);
    }
    private void Update()
    {
        if (sceneSwapInProgress)
        {
            currentAlpha = currentAlpha + Time.deltaTime * 0.5f;
            fadeImage.color = new Color(0, 0, 0, currentAlpha);
            if (currentAlpha >= 1.0F)
            {
                OP.SetGameMaster();
                SceneManager.LoadScene("Tutorial");
            }
        }
    }

    public void PlayGame()
    {

        sceneSwapInProgress = true;
        fadeImage.raycastTarget = true;
    }

    public void QuitButton()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
