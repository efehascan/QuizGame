using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
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

    private Coroutine activeTimerCoroutine;

    public void StartQuestionTimer()
    {
        if (activeTimerCoroutine != null)
            StopCoroutine(activeTimerCoroutine);

        activeTimerCoroutine = StartCoroutine(QuestionTimer());
    }

    private IEnumerator QuestionTimer()
    {
        float maxTime = 30f;
        float startTime = Time.time;

        while (Time.time - startTime < maxTime)
        {
            yield return null;
        }

        Debug.Log("Süre doldu, cevap verilmedi!");
        ScoreManager.Singleton.HandleAnswer(false, maxTime + 1);
        //UIManager.Singleton.ForceClosePanel();
    }

    public void StopTimerAndCheckAnswer(bool isCorrect)
    {
        if (activeTimerCoroutine != null)
        {
            StopCoroutine(activeTimerCoroutine);
        }
    }
}
            