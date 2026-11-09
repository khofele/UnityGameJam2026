using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class camMovement : MonoBehaviour
{
    [SerializeField] private float m_angleHor = 0f;
    [SerializeField] private float m_radius = 10f;
    [SerializeField] private float m_rotation = 400f;
    [SerializeField] private float m_angleVer = 0f;
    
    private void Start()
    {
        m_angleVer = Mathf.Asin(transform.position.y / m_radius) * Mathf.Rad2Deg;
        m_angleHor = Mathf.Atan2(transform.position.x, transform.position.z) * Mathf.Rad2Deg;
    }

    private void Update()
    {

        if (Keyboard.current.aKey.isPressed == true)
        {
            m_angleHor += m_rotation * Time.deltaTime;
        }
        if (Keyboard.current.dKey.isPressed == true)
        {
            m_angleHor -= m_rotation * Time.deltaTime;
        }


        if (Keyboard.current.wKey.isPressed == true)
        {
            if ((m_angleVer + m_rotation * Time.deltaTime) < 90 == true)
            {
                m_angleVer += m_rotation * Time.deltaTime;
            }
                
        }
        if (Keyboard.current.sKey.isPressed == true)
        {
            if (m_angleVer - m_rotation * Time.deltaTime > 45)
            {
                m_angleVer -= m_rotation * Time.deltaTime;
            }
                
        }
        

        float x = m_radius * Mathf.Cos(m_angleVer * Mathf.Deg2Rad) * Mathf.Sin(m_angleHor * Mathf.Deg2Rad);
        float y = m_radius * Mathf.Sin(m_angleVer * Mathf.Deg2Rad);
        float z = m_radius * Mathf.Cos(m_angleVer * Mathf.Deg2Rad) * Mathf.Cos(m_angleHor * Mathf.Deg2Rad);

        
        transform.position = new Vector3(x, y, z);
        transform.LookAt(Vector3.zero);
    }
}
