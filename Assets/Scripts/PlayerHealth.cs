using Michsky.MUIP;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public GameObject DeathCamera;
    public ProgressBar hp;
    public bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        var s = Instantiate(GetComponent<PlayerMovement>().bloodParticle, transform);
        Destroy(s, 0.5f);
       
    }

    private void Update()
    {
        hp.currentPercent = currentHealth;

        if (currentHealth < 0)
        {
            Die();
            currentHealth = 0;
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            DeathCamera.SetActive(true);
            Camera.main.gameObject.SetActive(false);
            GetComponent<PlayerMovement>().enabled = false;
            StartCoroutine(end_scene());
            currentHealth = 0;
            FindObjectOfType<AudioManager>().die.Play();
        }
       
    }

    IEnumerator end_scene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
