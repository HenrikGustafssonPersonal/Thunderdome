using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreenScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Image DyingStatusEffectRed;
    [SerializeField]
    private Image BlackScreen;
    [SerializeField]
    private TMP_Text deathTitle;

    [SerializeField]
    private TMP_Text mainMenuText;
    [SerializeField]
    private TMP_Text restartText;
    [SerializeField]
    private TMP_Text rageQuitText;


    public Vector3 startScale = new Vector3(6.22f, 6.22f, 6.22f);
    public Vector3 finalScale = new Vector3(3, 3, 3);


    private float currentDamageOpacity;
    public float redScreenScaleDuration = 2f;
    public float redScreenToBlackDuration = 2f;
    public float menuTextAppearDuration = 0.4f;
    private Color finalBlackImage;
    private Color fullOpacityMenuText;
    private Color fullOpacityRQText;
    private Vector3 rageQuitOrgPos;
    private string story;


    void Start()
    {
        //Begin dying effect with same opacity
        //currentDamageOpacity = gameObject.GetComponent<DamagedScreenEffect>().currentOpacity;
        currentDamageOpacity = GameManager.instance.getCurrentScreenDamageOpacity();
        Color tempVec = Color.red;
        tempVec.a = currentDamageOpacity;
        finalBlackImage = BlackScreen.color;
        finalBlackImage.a = 1;
        Color mainMenuTextColorVec = mainMenuText.color;
        Color rqTextColorVec = rageQuitText.color;
        mainMenuTextColorVec.a = 0;
        rqTextColorVec.a = 0;
        //Menu opacity assignments
        mainMenuText.color = mainMenuTextColorVec;
        restartText.color = mainMenuTextColorVec;
        rageQuitText.color = rqTextColorVec;
        rageQuitOrgPos = rageQuitText.transform.position;
        //Final Opacity
         fullOpacityMenuText = mainMenuText.color;
        fullOpacityMenuText.a = 1;

        fullOpacityRQText = rageQuitText.color;
        fullOpacityRQText.a = 1;

        story = deathTitle.text;

        deathTitle.text = "";

        DyingStatusEffectRed.transform.localScale = startScale;
        DyingStatusEffectRed.color = tempVec;
        
        fadeInDyingStatusEffect();
    }
    private void Update()
    {
       
        rageQuitText.transform.position = new Vector3(rageQuitOrgPos.x + Random.Range(-1, 1), rageQuitOrgPos.y + Random.Range(-1, 1));
    }
    private void fadeInDyingStatusEffect()
    {
        //fade scale from current scale to 3

        DOTween.To(() => DyingStatusEffectRed.transform.localScale, x => DyingStatusEffectRed.transform.localScale = x, finalScale, redScreenScaleDuration).SetEase(Ease.InOutSine);
        
        DOTween.To(() => DyingStatusEffectRed.color, x => DyingStatusEffectRed.color = x, Color.black, redScreenScaleDuration).SetEase(Ease.InOutSine);

        DOTween.To(() => BlackScreen.color, x => BlackScreen.color = x, finalBlackImage, redScreenScaleDuration).SetEase(Ease.InOutSine).OnComplete(()=>
        {

            StartCoroutine("PlayText");

         

        });


    }

    public void RestartGame()
    {
        if (TutorialManager.Instance != null)
        {
            Destroy(TutorialManager.Instance.gameObject);

        }

        SceneManager.LoadScene("Tutorial");
    }
    public void MainMenu()
    {
        if (AudioManager.Instance != null) Destroy(AudioManager.Instance.gameObject);
        if (TutorialManager.Instance != null) Destroy(TutorialManager.Instance.gameObject);

        SceneManager.LoadScene("MainMenu");

    }
    public void QuitButton()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    IEnumerator PlayText()
    {

        bool insertEffect = true;
        foreach (char c in story)
        {
            deathTitle.text += c;
            yield return new WaitForSeconds(0.125f);
        }
        Debug.Log("Done printing evis");
        //Chaining menu text appearance
        DOTween.To(() => restartText.color, x => restartText.color = x, fullOpacityMenuText, menuTextAppearDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            DOTween.To(() => mainMenuText.color, x => mainMenuText.color = x, fullOpacityMenuText, menuTextAppearDuration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                DOTween.To(() => rageQuitText.color, x => rageQuitText.color = x, fullOpacityRQText, menuTextAppearDuration * 4).SetEase(Ease.InOutSine);
            });
        
        }
        );

        while (true)
        {
            if (insertEffect)
            {
                deathTitle.text = "eviscerated_";
                insertEffect = false;
                yield return new WaitForSeconds(0.500f);

            }
            else
            {
                deathTitle.text = "eviscerated";
                insertEffect = true;
                yield return new WaitForSeconds(0.500f);
            }
        }
    }   
}
