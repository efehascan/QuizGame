using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
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
    
    private string apiUrl = "https://api.jsonbin.io/v3/b/67e10cf68a456b79667b81b0";
    private string accessKey = "$2a$10$MdpLLQ6zOmvkTiosYEK2y.pe3weEXrXaWd/u4w4D3JFX6iDyedeKO";
    private string json;
    
    private List<Question> allQuestions = new List<Question>();


    private void Start()
    {
        StartCoroutine(FetchQuestions());
        

    }


    /// <summary>
    /// API üzerinden JSON formatındaki soruları çeker ve oyun içi soru listesine yükler.
    /// Başarıyla yüklenirse rastgele bir soruyu otomatik olarak ekrana getirir.
    /// Hata oluşursa ilgili hata mesajını konsola yazdırır.
    /// </summary>
    IEnumerator FetchQuestions()
    {
        UnityWebRequest wwwRequest = UnityWebRequest.Get(apiUrl);
        wwwRequest.SetRequestHeader("X-Access-Key", accessKey);
        
        yield return wwwRequest.SendWebRequest();

        if (wwwRequest.result == UnityWebRequest.Result.Success)
        {
            json = wwwRequest.downloadHandler.text;
            
            QuestionDataWrapper wrapper = JsonUtility.FromJson<QuestionDataWrapper>(json);

            if (wrapper != null && wrapper.record != null && wrapper.record.questions != null)
            {
                allQuestions = new List<Question>(wrapper.record.questions);
                Debug.Log("Soru sayısı: " + allQuestions.Count);
                
            }
            else
            {
                Debug.LogError("Soru listesi boş veya geçersiz. JSON formatını kontrol et!");
                Debug.Log("Gelen JSON:\n" + json); // Ekstra debug satırı
            }

            
        }
        else
        {
            Debug.Log("Soru çekme hatası " + wwwRequest.error);
        }
    }


    /// <summary>
    /// Soru listesinden rastgele bir soru seçer, seçilen soruyu listeden kaldırır ve döndürür.
    /// Eğer soru kalmadıysa null döner ve konsola uyarı mesajı yazdırır.
    /// </summary>
    /// <returns>Rastgele seçilen bir soru veya null.</returns>
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
    
    /// <summary>
    /// Rastgele bir soru seçer ve UIManager üzerinden kullanıcıya gösterir.
    /// Eğer soru yoksa herhangi bir işlem gerçekleştirmez.
    /// </summary>
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
    public string questionText;
    public string answer;
    public string[] options;
}
