using UnityEngine;

public class SBgrapplinghooksKin : SteeringBehaviourKin
{
    private Path m_path = new();
    private bool m_loop;
    [SerializeField] private float m_distanceReached = 0.1f;
    private int m_indexCurrentTarget = 1; //darf nicht 0 sein, später fixen

    public SBgrapplinghooksKin(Path _path, bool _loop = false, int _indexCurrentTarget = 1)
    {
        m_path = _path;
        m_loop = _loop;
        m_indexCurrentTarget = _indexCurrentTarget;
    }

    protected override Vector3 UpdateVelocity()
    {
        //throw new System.NotImplementedException();
        Vector3 v = Vector3.zero;

        if (m_path == null || m_path.GetSize() == 0)
            return v;

        m_target = m_path.GetNodeAt(m_indexCurrentTarget).Position;

        Vector3 currentPos = m_kinematics.Position;
        currentPos.y = 0;
        Vector3 targetPos = m_target;
        targetPos.y = 0;

        float sqrDistance = (currentPos - targetPos).sqrMagnitude;
        float sqrDistanceReached = m_distanceReached * m_distanceReached;
        //Debug.Log($"Index: {m_indexCurrentTarget} | Distanz: {sqrDistance / sqrDistanceReached}");
        if (sqrDistance < sqrDistanceReached)
        {
            if (m_indexCurrentTarget < m_path.GetSize() - 1)
            {
                m_indexCurrentTarget++;
                m_target = m_path.GetNodeAt(m_indexCurrentTarget).Position;
                Debug.Log("Zeues Ziel: " + m_target);
                Debug.Log("Index: " + m_indexCurrentTarget);
            }
            else
            {
                Debug.Log("Angekommen");
                Pausieren();
                //Drop Behaviour
                //GraphManager sagen wo man steht, damit last Position blockiert wird
            }
        }
        v = (m_target - m_kinematics.Position);
        if (v != Vector3.zero)
        {
            v = v.normalized * m_kinematics.MaxMoveVelo;
        }
        v.y = 0f;
        return v;
    }
}
