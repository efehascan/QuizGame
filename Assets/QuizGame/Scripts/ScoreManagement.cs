using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    private int score = 0;
    private int comboCount = 0;
    private bool isComboActive = false;

    private const int baseScore = 5;
    private const float fastAnswerThreshold = 3f;

    public void HandleAnswer(bool isCorrect, float timeTaken)
    {
        if (!isCorrect || timeTaken > 30f)
        {
            GameOver();
            return;
        }

        if (timeTaken <= fastAnswerThreshold)
        {
            comboCount++;

            if (comboCount >= 3)
                isComboActive = true;
        }
        else
        {
            comboCount = 0;
            isComboActive = false;
        }

        if (isComboActive)
        {
            int comboBonus = (comboCount - 2) * baseScore;
            score += baseScore + comboBonus;
        }
        else
        {
            score += baseScore;
        }

        Debug.Log($"Skor: {score} | Kombo: {comboCount} | Süre: {timeTaken}");
    }

    private void GameOver()
    {
        Debug.Log("Oyun bitti!");
        PlayerController.Singleton.DeathPath();
    }
}