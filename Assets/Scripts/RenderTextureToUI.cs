using UnityEngine;

public class RenderTextureToUI : MonoBehaviour  //revenaufwendig
{
    [SerializeField] private Camera m_textureRenderCam = null;
    private RenderTexture m_renderTexture;
    public RenderTexture RenderTexture { get { return m_renderTexture; } }
    void Start()
    {
        m_renderTexture = new RenderTexture(512, 512, 32, RenderTextureFormat.ARGB32);
        m_renderTexture.Create();

        if (m_textureRenderCam != null)
        {
            m_textureRenderCam.targetTexture = m_renderTexture;
            m_textureRenderCam.clearFlags = CameraClearFlags.SolidColor;//Background clear with solid color
            m_textureRenderCam.backgroundColor = new Color(0f, 0f, 0f, 0f);//Color transparent
        }
    }
    private void OnDestroy()
    {
        if (m_renderTexture != null)
        {
            m_renderTexture.Release();
            Destroy(m_renderTexture);
        }
    }
}
