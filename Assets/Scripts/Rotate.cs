using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float m_rotationSpeed = 15f;
    void Update()
    {
        transform.Rotate(0f, m_rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
