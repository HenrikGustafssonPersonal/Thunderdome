using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
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

    public enum TutorialState{waitForDash, waitForItemPickUp, waitForSlide, waitForJump, waitForBomb, waitForWallJump, done};
    public TutorialState currentState;

    public GameObject StartPosBlocker;
    public GameObject WallSlideBlocker;

    public ParticleSystem glassShatter;

    // Start is called before the first frame update
    void Start()
    {
        currentState = TutorialState.waitForDash;
        tutorialText.SetText("Hold (W) and press (leftShift) to preform a dash forward");
        SetGUI(false);
        //gm = GunManagerGameObject.GetComponent<GunManager>();
        GunManagerGameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TutorialState.waitForDash:
                WaitForDash();
                break;
            case TutorialState.waitForItemPickUp:
                break;
            case TutorialState.waitForSlide:
                break;
            case TutorialState.waitForJump:
                break;
            case TutorialState.waitForBomb:
                WaitForBomb();
                break;
            case TutorialState.waitForWallJump:
                break;
            case TutorialState.done:
                break;
            default:
                break;
        }
    }

    private void WaitForDash()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                tutorialText.SetText("Pick up items on the floor by walking over them");
                glassShatter.Play();
                Destroy(StartPosBlocker);
                currentState = TutorialState.waitForItemPickUp;

            }
        }
        
    }

    public void ItemPickUp()
    {
        currentState = TutorialState.waitForSlide;
        tutorialText.SetText("Hold (W) and (leftCtrl) to preform a slide forward");
        Destroy(WallSlideBlocker);
        SetGUI(true);
        GunManagerGameObject.SetActive(true);
        GunManagerGameObject.GetComponent<GunManager>().UnequipAllGuns();
    }

    
    public void SlideDone()
    {
        currentState = TutorialState.waitForJump;
        tutorialText.SetText("Press (Space) while sliding to preform a higher jump");
    }
    public void JumpDone()
    {
        currentState = TutorialState.waitForBomb;
        tutorialText.SetText("Weapons have special abilities. Press (1) to equip your handgun.");
    }
    public void WaitForBomb()
    {
        if(GunManagerGameObject.GetComponent<GunManager>().currentWeapon == GunManager.WeaponType.HandGun)
            tutorialText.SetText("Throw a bomb with (Right Click) and then shoot it to make it explode");
        else
            tutorialText.SetText("Weapons have special abilities. Press (1) to equip your handgun.");
    }

    public void BombDone()
    {
        currentState = TutorialState.waitForWallJump;
        tutorialText.SetText("You can jump on walls by pressing space while next to them");
    }

    public void WallJumpDone()
    {
        currentState = TutorialState.done;
        tutorialText.SetText("");
    }
    private void SetGUI(bool enabled)
    {
        foreach (GameObject g in GUIGameObjects)
        {
            g.SetActive(enabled);
        }
    }
}
