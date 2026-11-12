using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public bool isBase = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " получил урон. Здоровье: " + currentHealth);

        if (currentHealth <= 0)
        {
            if (isBase)
            {
                bool isVictory = !gameObject.CompareTag("TeamAlly");
                GameManager.instance.EndGame(isVictory);
            }
            else if (GetComponent<HeroControll>() != null)
            {
                GameManager.instance.AddHeroDeath();
                GetComponent<HeroControll>().DieAndRespawn();
            }
            else
            {
                if (gameObject.CompareTag("TeamEnemy"))
                {
                    GameManager.instance.AddKill();
                }
                Destroy(gameObject);
            }
        }
    }
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
