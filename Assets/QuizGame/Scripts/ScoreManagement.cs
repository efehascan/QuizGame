using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
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
    #endregion

    [SerializeField] private TextMeshProUGUI scoreText;

    private int score = 0;
    private int comboCount = 0;
    private int basePoint = 5;
    private float fastAnswerThreshold = 3f; // 3 saniye içinde cevap

    private float lastAnswerTime;

    /// <summary>
    /// Oyun başladığında puan sıfırlanır.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
        comboCount = 0;
        UpdateScoreText();
    }

    /// <summary>
    /// Her doğru cevaptan sonra çağrılır. Cevap süresine göre puan hesaplanır.
    /// </summary>
    public void AddScore()
    {
        float timeTaken = Time.time - lastAnswerTime;

        if (timeTaken <= fastAnswerThreshold)
        {
            comboCount++;
        }
        else
        {
            comboCount = 0; // kombo bozuldu
        }

        int earned = basePoint;
        if (comboCount >= 3)
        {
            earned += comboCount * basePoint - 10; // 4. soru 10, 5. soru 15...
        }

        score += earned;
        UpdateScoreText();
        lastAnswerTime = Time.time;
    }

    /// <summary>
    /// Yeni soruya geçerken çağırılır, sürenin başladığı zamanı kaydeder.
    /// </summary>
    public void MarkAnswerStart()
    {
        lastAnswerTime = Time.time;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puan: " + score;
        }
    }
}