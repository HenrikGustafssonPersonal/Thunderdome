using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStats : MonoBehaviour
{

    public float maxStamina = 1000f;
    public float currentStamina;
    public float StaminaRegenRate =1f;

    public float maxHealth = 100;
    public float currentHealth;
    public float targetHealth;
    public bool godMode;

    public GameObject weaponAxe;

    public float grenadeCooldown = 10.0f;
    public float currentGrenadeCooldown = 0.0f;

    public float hookCooldown = 10.0f;
    public float currentHookCooldown = 0.0f;
    private bool alive;
    void Start()
    {
        currentStamina = maxStamina;
        currentHealth = maxHealth;
        targetHealth = maxHealth;
        alive = true;

        GameManager.instance.setHookSpecialBarMax(hookCooldown);
        GameManager.instance.setGrenadeSpecialBarMax(grenadeCooldown);
    }

    void Update()
    {

        //Pause functionality
        if (Input.GetKeyDown(KeyCode.P)) GameManager.instance.enableDisablePauseState();

        StaminaControls();
        SpecialHookControls();
        SpecialGrenadeControls();
        DamagedScreenControls();
        GameManager.instance.setStaminaBar(currentStamina);
        GameManager.instance.setCurrentHealthBar(currentHealth);
        GameManager.instance.setTargetHealthBar(targetHealth);

        currentHealth = Mathf.Lerp(currentHealth, targetHealth, Time.deltaTime * 10.0f);


        if (targetHealth <= 0.1f && alive == true)
        {
            Debug.Log("Player Died");
            
            GameManager.instance.enableDeathState();
            alive = false;
        }
    }

    public void StaminaControls()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina+= StaminaRegenRate*Time.deltaTime;

        }

    }
    public void SpecialHookControls ()
    {
        if (currentHookCooldown > 0.0f)
            currentHookCooldown -= Time.deltaTime;
        else
            currentHookCooldown = 0.0f;

        GameManager.instance.setHookSpecialBarCooldown(hookCooldown - currentHookCooldown);
    }

    public void DamagedScreenControls()
    {
        float peakOpacity = 1/(currentHealth/10);

        float minOpacity = peakOpacity - 0.25f;
        float intensity = 0.6f - (currentHealth / 200f);
    
        if (peakOpacity > 1)
        {
            peakOpacity = 1.0f;
        }

        if (intensity < 0.3f)
        {
            intensity = 0.3f;
        }


        if (currentHealth < 50.0f)
        {

            GameManager.instance.setPeakMinPulseOpacity(minOpacity, peakOpacity, intensity);

        } else
        {
            GameManager.instance.setPeakMinPulseOpacity(0, 0, 0);

        }

    }

    public void SpecialGrenadeControls()
    {

        if (currentGrenadeCooldown > 0.0f)
            currentGrenadeCooldown -= Time.deltaTime;
        else
            currentGrenadeCooldown = 0.0f;

        GameManager.instance.setGrenadeSpecialBarCooldown(grenadeCooldown - currentGrenadeCooldown);

    }
    public float GetStamina()
    {
        return currentStamina;
    }
    public void RemoveStamina(float staminaReduction)
    {
        if (staminaReduction < currentStamina)
        {
            DOTween.To(() => currentStamina, x => currentStamina = x, currentStamina - staminaReduction, 0.2f);

        }
    }
   
    public float GetHealth()
    {
        return currentHealth;
    }

    //For dashing,
    public void EnableGodModeDuration(float duration)
    {
        godMode = true;
        var val = 0;
        DOTween.To(() => val, x => val = x, 1, duration).OnComplete(() => godMode = false);

    }
    public void PlayerDamage(float damage)
    {
        if (godMode) return;

        if (targetHealth > damage)
        {
            targetHealth -= damage;


        }else if(damage >= targetHealth)
        {
            targetHealth = 0;
        }

    }
    public void PlayerHeal(float healAmount)
    {
        //Healing should only occur when axe is equipped

        if (weaponAxe.activeInHierarchy)
        {
            float missingHealth = maxHealth - targetHealth;
            if (missingHealth > healAmount)
            {
                targetHealth += healAmount;

            }
            else
            {
                targetHealth = maxHealth;
            }
        }


    }

}
