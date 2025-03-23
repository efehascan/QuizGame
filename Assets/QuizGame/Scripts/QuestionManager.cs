using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    private string apiUrl = "https://api.jsonbin.io/v3/b/67cf1c468561e97a50e979fb";
    private string accessKey = "$2a$10$5LpMum5j/QxfPEM3E.VMAuVga8aUzP/sqsGEEHiMeHKTZbEezXfDO";
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