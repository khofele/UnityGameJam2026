using UnityEngine;
using UnityEngine.InputSystem;

public class CamMovement : MonoBehaviour
{
    //private Vector3 m_target;
    [SerializeField] private GameObject m_player;

    [SerializeField] private float m_angleHor = 0f;
    [SerializeField] private float m_radius = 10f;
    [SerializeField] private float m_rotation = 400f;
    [SerializeField] private float m_angleVer = 0f;
    [SerializeField] private GameObject m_spielfeld;

    [SerializeField] private InputActionReference camMovWASD;
    [SerializeField] private InputActionReference camMovPointerDelta;
    [SerializeField] private InputActionReference camMovClickHold;
    private Vector2 camVector;
    


    private void Start()
    {
        //m_cam = Camera.main;
        m_angleVer = Mathf.Asin(transform.position.y / m_radius) * Mathf.Rad2Deg;
        m_angleHor = Mathf.Atan2(transform.position.x, transform.position.z) * Mathf.Rad2Deg;
        //m_target = new Vector3((m_spielfeld.transform.localScale.x * 12f) / 2f, 0f, (m_spielfeld.transform.localScale.z * 12f) / 2f) - new Vector3(0.5f, 0f, 0.5f); //target soll mitte vom spielfeld sein, funktioniert wsl nicht
        //m_target = _player.transform.position;
    }

    private void Update()
    {
        camVector = camMovWASD.action.ReadValue<Vector2>();

        if (camMovClickHold.action.IsPressed()/*)*/ && camVector == Vector2.zero)
        {
            camVector = camMovPointerDelta.action.ReadValue<Vector2>();
        }
        
        if (camVector.magnitude > 1f)
        {
            camVector.Normalize();
        }

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


        transform.position = new Vector3(x, y, z) + m_player.transform.position;
        transform.LookAt(m_player.transform.position);//Vector3.zero

    }

    private void OnEnable()
    {
        camMovWASD.action.Enable();
        camMovPointerDelta.action.Enable();
        camMovClickHold.action.Enable();
    }
    private void OnDisable()
    {
        camMovWASD.action.Disable();
        camMovPointerDelta.action.Disable();
        camMovClickHold.action.Disable();
    }
}
//es w�r nice to have, wenn die Kamera bei einem steileren winkel auch weiter weg ist, oder FOV halt anpassen
//so dass man mehr sieht, wenn weiter weg