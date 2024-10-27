using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening;

public class ArenaTutorialManager : MonoBehaviour
{
    public static ArenaTutorialManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public GameObject[] GUIGameObjects;
    public GameObject GunManagerGameObject;
    private GunManager gm;
    public TextMeshProUGUI tutorialText;

    public enum TutorialState{waitForGong, waitForShotGun, waitForShotgunText, waitForAxe, waitForAxeText, waitForCheerScoreText, done};
    public TutorialState currentState;
    public bool gongBeenShot = false;
    // Start is called before the first frame update
    void Start()
    {
        tutorialText.SetText("Shoot the gong to start the first round");

        currentState = TutorialState.waitForGong;
        

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TutorialState.waitForGong:
                WaitForGong();
                break;
            case TutorialState.waitForShotGun:
                WaitForShotGunEquip();
                break;
            case TutorialState.waitForAxe:
                WaitForAxeEquip();
                break;
            case TutorialState.done:
                break;
            default:
                break;
        }
    }
    private void WaitForGong()
    {
        if (gongBeenShot)
        {
            tutorialText.SetText("PRESS 2 TO EQUIP SHOTGUN");

            currentState = TutorialState.waitForShotGun;
        }
    }
    private void WaitForShotGunEquip()
    {

        if (GunManagerGameObject.GetComponent<GunManager>().currentWeapon == GunManager.WeaponType.ShotGun)
        { 
            currentState = TutorialState.waitForAxe;
            
            tutorialText.SetText("USING SPECIAL FIRE WILL PULL ENEMIES TOWARDS YOU");
            float timer = 1.0f;

            DOTween.To(() => timer, x => timer = x, 2.0f, 5).OnComplete(() =>
            {
                tutorialText.SetText("PRESS 3 TO EQUIP AXE");

            });
        }
    }

 
    
    public void WaitForAxeEquip()
    {

        if (GunManagerGameObject.GetComponent<GunManager>().currentWeapon == GunManager.WeaponType.Axe)
        {
            currentState = TutorialState.done;

            tutorialText.SetText("ATTACKING WITH THE AXE REGENERATES LOST HEALTH. EXECUTING ENEMIES WITH THE AXE GENERATES ADDITIONAL HEALTH");
            float timer = 1.0f;

            DOTween.To(() => timer, x => timer = x, 2.0f, 5).OnComplete(() =>
            {
                tutorialText.SetText("ELIMINATING ENEMIES IN QUICK SUCCESSION ADDS TO YOUR COMBO METER, EARNING YOU MORE POINTS.");

                float timer = 1.0f;

                DOTween.To(() => timer, x => timer = x, 2.0f, 5).OnComplete(() =>
                {
                    tutorialText.SetText("");

                });
            });
        }
    }


    public void SetGongShot()
    {
        if (!gongBeenShot) gongBeenShot = true;
    }

}
