using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    public static TimeManager Singleton { get; private set; }

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

    [SerializeField] private TextMeshProUGUI timeText;

    private float questionTime = 30f;
    private Coroutine timerCoroutine;

    public Action OnTimeUp;

    /// <summary>
    /// Sorunun süresini başlatır ve UI'ya saniye olarak yazar.
    /// </summary>
    public void StartTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(Countdown());
    }

    /// <summary>
    /// Süreyi durdurur.
    /// </summary>
    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }

    private IEnumerator Countdown()
    {
        float remainingTime = questionTime;

        while (remainingTime > 0)
        {
            timeText.text = Mathf.Ceil(remainingTime).ToString();
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        timeText.text = "0";
        OnTimeUp?.Invoke(); // Süre bittiğinde event tetikle
    }
}