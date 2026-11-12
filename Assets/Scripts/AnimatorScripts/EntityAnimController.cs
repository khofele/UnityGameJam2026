using UnityEngine;

[RequireComponent (typeof(Animator))]
public class EntityAnimController : MonoBehaviour
{
    private Animator m_animator;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    public void SetMovementParameter(bool isWalking)
    {
        Debug.Log("Setze Animation Laufen: " + isWalking);
        m_animator.SetBool("IsWalking", isWalking);
    }
    public void SetAttackTrigger()
    {
        m_animator.SetTrigger("Attack");
    }
    public void SetHitTrigger()//im damage dealer
    {
        m_animator.SetTrigger("Hit");
    }
    public void SetDeathTrigger()//auch damage dealer, OnDeath
    {
        m_animator.SetTrigger("Die");
    }
}
