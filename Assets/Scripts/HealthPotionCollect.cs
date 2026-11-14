using UnityEngine;

public class HealthPotionCollect : MonoBehaviour
{
    [SerializeField] private LayerMask m_LayerMask;
    public float Amount {  get; private set; }
    private void Start()
    {
        Amount = Random.value * 5f + 0.5f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((m_LayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            UIManager.Instance.ShowHealthPotion(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((m_LayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            UIManager.Instance.ClearHealthPotion();
        }
    }
}
