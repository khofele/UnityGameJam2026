using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pfadvisualisierung : MonoBehaviour
{
    LineRenderer line = null;
    [SerializeField] private GameObject spieler;
    private Path m_path = new Path();
    [SerializeField] private int m_size = 0;
    [SerializeField] private Material matT;
    [SerializeField] private Material matF;
    private void Start()
    {
        PathVis(m_path, false); 
    }

    private void Update()
    {
        
    }

    private void PathVis(Path _path, bool enoughAP)
    {
        line = gameObject.AddComponent<LineRenderer>();

        if (enoughAP)
            GetComponent<Renderer>().material = matT;//line.material = new Material(Shader.Find("lineRendererT_mat"));
        else GetComponent<Renderer>().material = matF;//line.material.color = Color.red;

        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        /*_path.AddNodeToPath(new Node(new Vector2(0, 0)));
        _path.AddNodeToPath(new Node(new Vector2(0, 1)));
        _path.AddNodeToPath(new Node(new Vector2(0, 2)));
        _path.AddNodeToPath(new Node(new Vector2(1, 2)));
        _path.AddNodeToPath(new Node(new Vector2(1, 3)));*/

        m_size = _path.GetSize();
        line.positionCount = m_size;

        for (int i = 0; i < m_size; i++)
        {
            Vector2 pos = _path.GetNodeAt(i).Position;
            //Debug.Log("Position Node " + i + ": " + pos);
            line.SetPosition(i, new Vector3(pos.x + spieler.transform.position.x, 0, pos.y + spieler.transform.position.z));
        }
    }
}
