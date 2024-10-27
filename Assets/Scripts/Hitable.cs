using UnityEngine;

public class Hitable : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public int playerHealAmountOnHit;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamge(float amout)
    {
        // If its already dead:
        if (currentHealth <= 0.0f) return;

        currentHealth -= amout;
        GameManager.instance.playerHeal(playerHealAmountOnHit);
        GameManager.instance.UI.GetComponentInChildren<HitMarker>().DoHitMarker();
        if(currentHealth <= 0f)
        {
            Destroy();
        }

        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
