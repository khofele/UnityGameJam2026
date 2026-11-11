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

    [SerializeField] private InputActionReference camMov;
    private Vector2 camVector;
    


    void Start()
    {
        m_angleVer = Mathf.Asin(transform.position.y / m_radius) * Mathf.Rad2Deg;
        m_angleHor = Mathf.Atan2(transform.position.x, transform.position.z) * Mathf.Rad2Deg;
        Target = new Vector3((m_spielfeld.transform.localScale.x * 12f) / 2f, 0f, (m_spielfeld.transform.localScale.z * 12f) / 2f) - new Vector3(0.5f, 0f, 0.5f); //target soll mitte vom spielfeld sein, funktioniert wsl nicht
    }

    private void Update()
    {
        camVector = camMov.action.ReadValue<Vector2>();

        m_angleHor -= m_rotation * Time.deltaTime * camVector.x;
        if (m_angleVer - m_rotation * Time.deltaTime > 45 && camVector.y < 0)
        {
            m_angleVer += m_rotation * Time.deltaTime * camVector.y;
        }
        if (m_angleVer + m_rotation * Time.deltaTime < 90 && camVector.y > 0)
        {
            m_angleVer += m_rotation * Time.deltaTime * camVector.y;
        }


        float x = m_radius * Mathf.Cos(m_angleVer * Mathf.Deg2Rad) * Mathf.Sin(m_angleHor * Mathf.Deg2Rad);
        float y = m_radius * Mathf.Sin(m_angleVer * Mathf.Deg2Rad);
        float z = m_radius * Mathf.Cos(m_angleVer * Mathf.Deg2Rad) * Mathf.Cos(m_angleHor * Mathf.Deg2Rad);

        
        transform.position = new Vector3(x, y, z) + Target;
        transform.LookAt(Target);//Vector3.zero
    }

    private void OnEnable()
    {
        camMov.action.Enable();
    }
    private void OnDisable()
    {
        camMov.action.Disable();
    }
}
//es w�r nice to have, wenn die Kamera bei einem steileren winkel auch weiter weg ist, oder FOV halt anpassen
//so dass man mehr sieht, wenn weiter weg