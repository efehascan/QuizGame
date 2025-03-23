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

    // ReSharper disable Unity.PerformanceAnalysis
    public void ShowQuestion(Question question)
    {
        currentQuestion = question;
        
        questionPanel.SetActive(true);
        questionText.text = question.question;

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
    
    private void OnOptionSelected(string selected)
    {
        if (selected == currentQuestion.answer)
        {
            Debug.Log("Doğru cevap verdin!");
        }
        else
        {
            Debug.Log("Yanlış cevap!");
        }

        questionPanel.SetActive(false);
    }
}
