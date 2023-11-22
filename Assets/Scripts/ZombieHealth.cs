using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth = 50;
    public float currentHealth;
    public GameObject zombieBlood;
    public GameObject deathEffect;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
