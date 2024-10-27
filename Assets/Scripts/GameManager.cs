using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private StaminaBar staminaBar;
    private HealthBar healthBar;
    private HookBar hookBar;
    private GrenadeBar grenadeBar;
    private PlayerParticleEffects playerParticleEffects;
    private CircleScript circleScript;
    private WaveSpawner waveSpawner;
    private CheerOMeter cheerOMeter;
    private CheerScore cheerScore;
    private DamagedScreenEffect damageScreenEffect;
    private DamagedDirection damageDirection;
    private Camera mainCamera;
    private WeaponBorderScript weaponBorderScript;
    private PlayerMovementScript playerMovement;
    private MouseLook playerMouseLook;
    private PlayerUI playerUI; //Player UI
    private PauseScreenScript pauseScreenScript;
    private GunManager gunManager;
    private PlayerStats playerStats;

    [SerializeField] GameObject VictoryScreen;
    [SerializeField] GameObject DeathScreen;

    [SerializeField] GameObject playerRef;
    public GameObject UI;

    private void Awake() 

    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        playerUI = UI.GetComponentInChildren<PlayerUI>();
        pauseScreenScript = UI.GetComponentInChildren<PauseScreenScript>();
        staminaBar = UI.GetComponentInChildren<StaminaBar>();
        healthBar = UI.GetComponentInChildren<HealthBar>();
        hookBar = UI.GetComponentInChildren<HookBar>();
        grenadeBar = UI.GetComponentInChildren<GrenadeBar>();
        damageScreenEffect = UI.GetComponentInChildren<DamagedScreenEffect>();
        cheerOMeter = UI.GetComponentInChildren<CheerOMeter>();
        damageDirection = UI.GetComponentInChildren<DamagedDirection>();
        weaponBorderScript = UI.GetComponentInChildren<WeaponBorderScript>();

        playerStats = playerRef.GetComponent<PlayerStats>();
        
        circleScript = gameObject.GetComponent<CircleScript>();
        playerParticleEffects = gameObject.GetComponent<PlayerParticleEffects>();
        waveSpawner = gameObject.GetComponent<WaveSpawner>();
        cheerScore= gameObject.GetComponent<CheerScore>();
        playerMovement = playerRef.GetComponent<PlayerMovementScript>();
        playerMouseLook = playerRef.GetComponentInChildren<MouseLook>();
        gunManager = playerRef.GetComponentInChildren<GunManager>();
        mainCamera = playerRef.GetComponentInChildren<Camera>();
        pauseScreenScript.gameObject.SetActive(false);


    }

    //Game Logic

    public void startNextRound()
    {
        if (ArenaTutorialManager.Instance != null) ArenaTutorialManager.Instance.SetGongShot();
        waveSpawner.BeginNextRound();
    }
    public void enableDeathState()
    {
        //Disable HUD, movement and gun manager.

        Vector3 deathPos = mainCamera.gameObject.transform.position - new Vector3(0, 3f, 0);

        DOTween.To(() => mainCamera.gameObject.transform.position, x => mainCamera.gameObject.transform.position = x, deathPos, 0.3f).SetEase(Ease.InOutSine);
        playerUI.gameObject.SetActive(false);
        gunManager.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        playerMovement.enabled = false;
        playerMouseLook.enabled = false;
        GameObject deathScreen = Instantiate(DeathScreen);
        deathScreen.transform.SetParent(UI.transform);


        //disable mouse look

    }
    public void enableWinState()
    {
        playerUI.gameObject.SetActive(false);
        gunManager.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        playerMovement.enabled = false;
        playerMouseLook.enabled = false;

        GameObject victoryScreen = Instantiate(VictoryScreen);
        victoryScreen.transform.SetParent(UI.transform);

    }
    public void enableDisablePauseState()
    {
        if (Time.timeScale != 0)
        {
            //enable pause
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            pauseScreenScript.gameObject.SetActive(true);


        }
        else
        {
            //disable pause
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            pauseScreenScript.gameObject.SetActive(false);

        }

    }
    //Player stats
    public void enableGodModeDuration(float duration)
    {
        playerStats.EnableGodModeDuration(duration);
    }
    public void removeStamina(float staminaReduction)
    {
        playerStats.RemoveStamina(staminaReduction);
    }
    public float getStamina()
    {
        return playerStats.GetStamina();
    }
    public void playerDamage(float damage)
    {
        playerStats.PlayerDamage(damage);

    }
    public void playerHeal(int healAmount)
    {
        playerStats.PlayerHeal(healAmount);

    }
    public void playerKnockBack(Vector3 knockBackDir, float knockBackForce)
    {
        playerMovement.ApplyKnockBack(knockBackDir,knockBackForce);

    }

    //Animations, maybe should be moved to player
    public void drawCircle(Vector3 currentPosition, float radius)
    {   
        circleScript.DrawCircle(currentPosition, radius);
    }
    public void playSpeedLineAnimation()
    {
        playerParticleEffects.PlaySpeedLineAnimation();
    }
    public void playFallLineAnimation()
    {
        playerParticleEffects.PlayFallLineAnimation();
    }

    public void stopFallLineAnimation()
    {
        playerParticleEffects.StopFallLineAnimation();
    }


    //UI Value assignments
    public void setCheerOMeter(float cheerScore)
    {
        cheerOMeter.SetPinRotation(cheerScore);
    }
    public void setTotalScoreText(int totalScore)
    {
        cheerOMeter.SetTotalScoreText(totalScore);
    }

    public void setScoreMultiplier(int scoreMultiplier)
    {
        cheerOMeter.SetScoreMultiplier(scoreMultiplier);
    }
    public void setHookSpecialBarCooldown(float coolDown)
    {
        hookBar.SetCooldown(coolDown);
    }
    public void setHookSpecialBarMax(float coolDown)
    {
        hookBar.SetMaxHookCooldown(coolDown);
    }

    public void setGrenadeSpecialBarCooldown(float coolDown)
    {
        grenadeBar.SetCooldown(coolDown);
    }
    public void setGrenadeSpecialBarMax(float coolDown)
    {
        grenadeBar.SetMaxGrenadeCooldown(coolDown);
    }
    public void setStaminaBar(float stamina)
    {
        staminaBar.SetStamina(stamina);
    }
    public void setMaxStaminaBar(float maxStamina)
    {
        staminaBar.SetMaxStamina(maxStamina);
    }

    public void setCurrentHealthBar(float health)
    {
        healthBar.SetCurrentHealth(health);
    }
    public void setTargetHealthBar(float health)
    {
        healthBar.SetTargetHealth(health);
    }
    public void adjustUiFromMovement(Vector3 movementX,Vector3 movementY)
    {
        hookBar.adjustMovement(movementX, movementY);
    }

    public void setPeakMinPulseOpacity(float minVal, float maxVal, float intensity)
    {
        //Adjust damaged screen effect
        damageScreenEffect.SetPeakMinPulseOpacity(minVal, maxVal, intensity);

    }

    public float getCurrentScreenDamageOpacity()
    {
        return damageScreenEffect.GetCurrentScreenDamageOpacity();
    }

    public void setCurrentWeaponBorder(int currentWeapon)
    {
     
            
        weaponBorderScript.SetCurrentWeaponBorder(currentWeapon);
        
            
        
    }

    public void drawDamageArrow(Vector3 enemyPosition)
    {
        damageDirection.DrawDamageIndicator(enemyPosition);
    }
    //Cheer score

    public void addToTotalScore(int amount)
    {
        cheerScore.AddToTotalScore(amount);
    }
    public void addToCheerScore(float amount)
    {
        cheerScore.AddToCheerScore(amount);
    }

    public int getScore()
    {
        return cheerScore.totalScore;
    }

}
