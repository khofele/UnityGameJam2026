using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pfadvisualisierung : MonoBehaviour
{
    LineRenderer line = null;
    [SerializeField] private GameObject spieler;
    private Path m_path = null;
    private int m_size = 0;
    private void Start()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.material.color = Color.purple;
    }

    private void Update()
    {
        transform.position = spieler.transform.position;
        m_size = m_path.GetSize();

        for (int i = 0; i < m_size; i++) {
            Vector2 pos = m_path.GetNodeAt(i).Position;
            line.SetPosition(i, new Vector3(pos.x, 0, pos.y)); 
        }
    }
}
