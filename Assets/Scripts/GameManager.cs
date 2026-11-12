using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI statsText;
    public int enemiesKilled = 0;
    public int heroDeaths = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        Time.timeScale = 1f;
    }

    public void AddKill()
    {
        enemiesKilled++;
    }

    public void AddHeroDeath()
    {
        heroDeaths++;
    }

    public void EndGame(bool isVictory)
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        if (isVictory)
        {
            resultText.text = "ПОБЕДА!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "ПОРАЖЕНИЕ";
            resultText.color = Color.red;
        }
        statsText.text = $"Врагов уничтожено: {enemiesKilled}\nСмертей героя: {heroDeaths}";
    }
}
