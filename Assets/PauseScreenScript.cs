using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenScript : MonoBehaviour
{
    public void MainMenu()
    {
        if (TutorialManager.Instance != null) Destroy(TutorialManager.Instance.gameObject);
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);


        GameManager.instance.enableDisablePauseState();
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("MainMenu");

    }
    public void ResumeGame()
    {
        Debug.Log("cliicked resume game");
        GameManager.instance.enableDisablePauseState();
    }
}
