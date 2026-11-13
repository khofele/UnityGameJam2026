using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class Pfadvisualisierung : MonoBehaviour
{
    private LineRenderer m_line;
    //private GameObject m_pieler;
    //private Path m_path = new Path();
    //[SerializeField] private int m_size = 0;
    [SerializeField] private Material matT;
    [SerializeField] private Material matF;
    [SerializeField] private Material m_gradientMaterial;

    private void Start()
    {
        if (m_line == null && TryGetComponent(out LineRenderer l))
        {
            m_line = l;
            //line.enabled = false;
        }
    }

    public void ShowPath(bool _showPath)
    {
        if (_showPath)
            m_line.enabled = true;
        else
            m_line.enabled = false;
    }
    //Nicht verwenden - veraltet und hat Memory Leak
    private void PathVis(Path _path, bool enoughAP)
    {
        ShowPath(true);
        if (m_line == null && TryGetComponent(out LineRenderer l))
        {
            m_line = l;
        }

        if (enoughAP)
        {
            GetComponent<Renderer>().material = matT;//line.material = new Material(Shader.Find("lineRendererT_mat"));
            //Memory Leak
        }
        else
        {
            GetComponent<Renderer>().material = matF;//line.material.color = Color.red;
            //Memory Leak
        }
        m_line.startWidth = 0.1f;
        m_line.endWidth = 0.1f;

        /*_path.AddNodeToPath(new Node(new Vector2(0, 0)));
        _path.AddNodeToPath(new Node(new Vector2(0, 1)));
        _path.AddNodeToPath(new Node(new Vector2(0, 2)));
        _path.AddNodeToPath(new Node(new Vector2(1, 2)));
        _path.AddNodeToPath(new Node(new Vector2(1, 3)));*/

        int size = _path.GetSize();
        m_line.positionCount = size;

        for (int i = 0; i < size; i++)
        {
            Vector2 pos = _path.GetNodeAt(i).Position;
            //Debug.Log("Position Node " + i + ": " + pos);
            m_line.SetPosition(i, new Vector3(pos.x, 0.1f, pos.y));
        }
    }
    public void DrawPath(Path _path, int _currentAP)
    {
        ShowPath(true);
        if (m_line == null && TryGetComponent(out LineRenderer l))
        {
            m_line = l;
            m_line.material = m_gradientMaterial;
        }

        m_line.startWidth = 0.1f;
        m_line.endWidth = 0.1f;
                
        int size = _path.GetSize();
        m_line.positionCount = size;

        if (size > 0)
        {
            Vector2 startPos = _path.GetNodeAt(0).Position;
            transform.position = new Vector3(startPos.x, 0.1f, startPos.y);
        }

        for (int i = 0; i < size; i++)
        {
            Vector2 pos = _path.GetNodeAt(i).Position;
            m_line.SetPosition(i, new Vector3(pos.x, 0.1f, pos.y));
        }

        SetPathColors(_path.GetSize(), _currentAP);
    }
    private void SetPathColors(int _pathNumberOfNodes, int _currentAP)
    {
        float percentage = (float)(_currentAP - 1) / (_pathNumberOfNodes - 1);
        percentage = Mathf.Clamp01(percentage);
        Debug.Log("Colorgrading Path Visualization: " + percentage);

        Gradient gradient = new();
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
        {
        new(1f, 0f),
        new(1f, 1f)
        };

        if (_currentAP - 1 >= _pathNumberOfNodes - 1)
            gradient.SetKeys(new GradientColorKey[] { new(Color.green, 0f), new(Color.green, 1f) }, alphaKeys);
        else if (_currentAP <= 0)
            gradient.SetKeys(new GradientColorKey[] { new(Color.red, 0f), new(Color.red, 1f) }, alphaKeys);
        else
        {
            GradientColorKey[] colorKeys = new GradientColorKey[4];
            colorKeys[0] = new GradientColorKey(Color.green, 0f);
            colorKeys[1] = new GradientColorKey(Color.green, percentage);
            colorKeys[2] = new GradientColorKey(Color.red, percentage + 0.001f);
            colorKeys[3] = new GradientColorKey(Color.red, 1f);

            gradient.SetKeys(colorKeys, alphaKeys);
        }

        m_line.colorGradient = gradient;
    }
}
