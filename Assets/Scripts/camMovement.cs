using UnityEngine;
using UnityEngine.InputSystem;

public class camMovement : MonoBehaviour
{
    public Vector3 Target;
    [SerializeField] private float m_angleHor = 0f;
    [SerializeField] private float m_radius = 10f;
    [SerializeField] private float m_rotation = 400f;
    [SerializeField] private float m_angleVer = 0f;
    [SerializeField] private GameObject m_spielfeld;

    [SerializeField] private GameObject m_spielfeld;
    
    void Start()
    {
        angleVer = Mathf.Asin(transform.position.y / radius) * Mathf.Rad2Deg;
        angleHor = Mathf.Atan2(transform.position.x, transform.position.z) * Mathf.Rad2Deg;
        target = new Vector3((m_spielfeld.transform.localScale.x * 12f) / 2f, 0f, (m_spielfeld.transform.localScale.z * 12f) / 2f) - new Vector3(0.5f, 0f, 0.5f); //target soll mitte vom spielfeld sein, funktioniert wsl nicht
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

        
        transform.position = new Vector3(x, y, z) + target;
        transform.LookAt(target);//Vector3.zero
    }
}
//es w�r nice to have, wenn die Kamera bei einem steileren winkel auch weiter weg ist, oder FOV halt anpassen
//so dass man mehr sieht, wenn weiter weg