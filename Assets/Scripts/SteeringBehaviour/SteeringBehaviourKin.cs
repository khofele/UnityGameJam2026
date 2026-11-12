using UnityEngine;

public abstract class SteeringBehaviourKin : Option
{
    protected Kinematics m_kinematics;
    protected Vector2 m_target;

    public event System.Action<bool> OnMovementStateChanged;

    public void SetKinematic(Kinematics kinematics)
    {
        m_kinematics = kinematics;
    }

    public void SetTarget(Vector3 target)   //unnötig?
    {
        m_target = target;
    }
    protected Vector3 Limit(Vector3 v, float max)
    {
        v = Vector3.ClampMagnitude(v, max);
        return v;
    }
    protected float Limit(float v_rot, float max)
    {
        v_rot = Mathf.Clamp(v_rot, -max, max);
        return v_rot;
    }
    protected abstract Vector3 UpdateVelocity();
    protected override void OnTick(float fTimeDelta)
    {
        if (CurrentZustand != Zustand.Gestartet) { /*Debug.Log("Zustand nicht gestartet");*/ return; }

        Vector3 v_t = UpdateVelocity();
        m_kinematics.MoveVelo = Limit(v_t, m_kinematics.MaxMoveVelo);
        
        float rad = Mathf.Atan2(v_t.x, v_t.z);
        float deltaRad = rad - m_kinematics.Rotation;
        deltaRad = Mathf.Repeat(deltaRad + Mathf.PI, Mathf.PI * 2f) - Mathf.PI;
        float targetRotVelo = deltaRad / fTimeDelta;
        m_kinematics.RotVelo = Limit(targetRotVelo, m_kinematics.MaxRotVelo);
        
        m_kinematics.Position += m_kinematics.MoveVelo * fTimeDelta;
        m_kinematics.Rotation += m_kinematics.RotVelo * fTimeDelta;

        m_kinematics.Rotation = Mathf.Repeat(m_kinematics.Rotation + Mathf.PI, Mathf.PI * 2f) - Mathf.PI;
    }
    protected override void OnPause()
    {
        OnMovementStateChanged?.Invoke(false);
    }
    protected override void OnStart()
    {
        OnMovementStateChanged?.Invoke(true);
    }
}
