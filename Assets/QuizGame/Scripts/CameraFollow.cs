using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Ayarları")]
    public Transform target; // Takip edilecek karakter
    public Vector3 offset = new Vector3(0, 5, -10); // Kameradan karaktere olan mesafe
    
    [Header("Hareket Ayarları")]
    public float followSpeed = 2f; // Takip hızı
    public bool smoothFollow = true; // Yumuşak takip
    
    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 desiredPosition = target.position + offset;
        
        if (smoothFollow)
        {
            // Yumuşak takip
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            // Direkt takip
            transform.position = desiredPosition;
        }
    } 
}
