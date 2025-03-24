using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointManager : MonoBehaviour
{
    #region Singleton
    public static MovePointManager Singleton { get; private set; }

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

    public void MovePoint(Transform targetTransform)
    {
        Vector3 newPos = targetTransform.position + new Vector3(2.5f, 0f, 0f);
        targetTransform.position = newPos;
    }
    
}
