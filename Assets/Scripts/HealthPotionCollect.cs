using UnityEngine;

public class HealthPotionCollect : MonoBehaviour
{
    //[SerializeField] private LayerMask m_LayerMask;
    public float Amount {  get; private set; }
    private EntityPlayer m_player;
    private Vector2 m_position2;
    private bool m_isInReach = false;
    private void Start()
    {
        Amount = Random.value * 5f + 0.5f;
        m_position2 = new(transform.position.x, transform.position.z);
        m_player = UIManager.Instance.Player;
    }
    private void Update()
    {
        float distance = Vector2.Distance(m_player.CurrentNodePos, m_position2);
        if (distance < 0.45f && m_isInReach == false)
        {
            UIManager.Instance.ShowHealthPotion(this);
            m_isInReach = true;
        }
        else if (distance > 0.5f && m_isInReach == true)
        {
            UIManager.Instance.ClearHealthPotion();
            m_isInReach = false;
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if ((m_LayerMask.value & (1 << other.gameObject.layer)) > 0)
    //    {
    //        UIManager.Instance.ShowHealthPotion(this);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if ((m_LayerMask.value & (1 << other.gameObject.layer)) > 0)
    //    {
    //        UIManager.Instance.ClearHealthPotion();
    //    }
    //}

}
