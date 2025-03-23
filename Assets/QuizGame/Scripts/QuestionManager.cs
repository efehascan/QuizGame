using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class QuestionManager : MonoBehaviour
{
    #region Singleton
    public static QuestionManager Singleton { get; private set; }

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
    
    private string apiUrl = "https://api.jsonbin.io/v3/b/67cf1c468561e97a50e979fb";
    private string accessKey = "$2a$10$5LpMum5j/QxfPEM3E.VMAuVga8aUzP/sqsGEEHiMeHKTZbEezXfDO";
    private string json;
    
    private List<Question> allQuestions = new List<Question>();


    private void Start()
    {
        StartCoroutine(FetchQuestions());
    }

    IEnumerator FetchQuestions()
    {
        UnityWebRequest wwwRequest = UnityWebRequest.Get(apiUrl);
        wwwRequest.SetRequestHeader("X-Access-Key", accessKey);
        
        yield return wwwRequest.SendWebRequest();

        if (wwwRequest.result == UnityWebRequest.Result.Success)
        {
            json = wwwRequest.downloadHandler.text;
            
            QuestionDataWrapper wrapper = JsonUtility.FromJson<QuestionDataWrapper>(json);

            if (wrapper != null && wrapper.record != null)
            {
                allQuestions = new List<Question>(wrapper.record.questions);
                Debug.Log("Soru sayısı: " + allQuestions.Count);
                
                AskRandomQuestion();
            }
            else
            {
                Debug.Log("JSON Dosyası yüklenmedi");
            }
            
        }
        else
        {
            Debug.Log("Soru çekme hatası " + wwwRequest.error);
        }
    }


    public Question GetRandomQuestion()
    {
        if (allQuestions.Count == 0)
        {
            Debug.Log("Tüm sorular kullanıldı");
            return null;
        }
        
        int randomIndex = Random.Range(0, allQuestions.Count);
        Question question = allQuestions[randomIndex];
        allQuestions.RemoveAt(randomIndex);
        
        return question;
    }
    
    public void AskRandomQuestion()
    {
        var q = QuestionManager.Singleton.GetRandomQuestion();
        if (q != null)
        {
            UIManager.Singleton.ShowQuestion(q);
        }
    }
        
}

[System.Serializable]
public class QuestionDataWrapper
{
    public QuestionData record;
}

[System.Serializable]
public class QuestionData
{
    public Question[] questions;
}

[System.Serializable]
public class Question
{
    public string question;
    public string answer;
    public string[] options;
}
