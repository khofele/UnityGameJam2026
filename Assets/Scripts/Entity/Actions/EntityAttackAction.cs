using UnityEngine;

[CreateAssetMenu(fileName = "EntityAttackAction", menuName = "Scriptable Entity Objects/EntityAttackAction")]
public class EntityAttackAction : EntityAction
{
    [SerializeField] private float m_damage = 0.0f;
    
    public float Damage
    {
        get { return m_damage; }
    }
}
