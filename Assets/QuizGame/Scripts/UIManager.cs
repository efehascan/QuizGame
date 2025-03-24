using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Singleton { get; private set; }

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
    
    [Header("UI Elements")]
    [SerializeField] GameObject questionPanel;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] Button[] optionButtons;
    
    private Question currentQuestion;

    
    /// <summary>
    /// Verilen soruyu UI üzerinde görüntüler, soru metnini ve cevap seçeneklerini günceller.
    /// Her butona ilgili cevap seçeneğini atar ve seçime göre cevap kontrolünü yapar.
    /// Kullanılmayan butonları gizler.
    /// </summary>
    /// <param name="question">Gösterilecek soru nesnesi.</param>
    public void ShowQuestion(Question question)
    {
        currentQuestion = question;
        
        questionPanel.SetActive(true);
        questionText.text = question.questionText;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < question.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = question.options[i];
                
                string selectedAnswer = question.options[i];
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener((() => OnOptionSelected(selectedAnswer)));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
        
        
    }
    
    /// <summary>
    /// Kullanıcının seçtiği cevabı doğru cevapla karşılaştırır.
    /// Sonuca göre doğru veya yanlış cevap mesajını konsola yazdırır.
    /// Ardından soru panelini kapatır.
    /// </summary>
    /// <param name="selected">Kullanıcının seçtiği cevap.</param>
    private void OnOptionSelected(string selected)
    {
        if (selected == currentQuestion.answer)
        {
            Debug.Log("Doğru cevap verdin!");
            PlayerController.Singleton.UnFreezePlayer();
        }
        else
        {
            Debug.Log("Yanlış cevap!");
        }

        questionPanel.SetActive(false);
    }
}
