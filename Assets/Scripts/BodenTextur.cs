using Unity.VisualScripting;
using UnityEngine;

public class BodenTextur : MonoBehaviour
{
   
//#if UNITY_EDITOR
//    private void OnValidate()
//    {
//        UnityEditor.EditorApplication.delayCall -= _OnValidateCallback;
//        UnityEditor.EditorApplication.delayCall += _OnValidateCallback;

//    }
//    private void _OnValidateCallback()
//    {
//        Vector2 ScaleObj = transform.localScale;
//        Renderer rend = GetComponent<Renderer>();

//        rend.sharedMaterial.mainTextureScale = ScaleObj;
//    }
//#endif

    private void Start()
    {
        Vector2 ScaleObj = transform.localScale;
        Renderer rend = GetComponent<Renderer>();

        rend.sharedMaterial.mainTextureScale = ScaleObj;
    }

}
