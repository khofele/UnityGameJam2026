using UnityEngine;

public class PlayingFieldManager : MonoBehaviour
{
    public float size = 1;
    public static PlayingFieldManager instance = null;
    public Vector3 offset = Vector3.zero;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        transform.localScale = new (size, size, size);
        transform.position = offset;
    }
}
