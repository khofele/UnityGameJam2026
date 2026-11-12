using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

//!!!!!!!!!!!!!!!!!!!!!MenuBG Tag setzen!!!!!!!!!!!!!
[RequireComponent(typeof(EntityPlayer))]
public class PlayerMovement : MonoBehaviour
{
    //Ronja Code zum vergleichen

    //Vector3 newPosition;
    //private Vector3 speed = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    newPosition = transform.position;
    //}



    // Update is called once per frame
    //void Update()
    //{
    //    if (Mouse.current.leftButton.wasPressedThisFrame)//lieber mit input action
    //    {
    //        RaycastHit hit;
    //        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
    //        if (Physics.Raycast(ray, out hit))//�ber graph manager checken ob man da �berhaupt hin kann oder ob's ein gegner ist per Layermask
    //        {
    //            newPosition = new Vector3(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));//war Round()
    //            Debug.Log(newPosition);
    //        }
    //    }
    //    transform.position = Vector3.SmoothDamp(transform.position,newPosition, ref speed, 0.3f);
    //}
    private CamMoveAndClick_Actions m_inputActions;
    [SerializeField] private LayerMask m_clickableLayers;
    private EntityPlayer m_playerEntity;
    private List<RaycastResult> m_raycastResults = new List<RaycastResult>();
    Vector2 m_screenPos;
    bool m_screenClicked = false;
    private void Awake()
    {
        m_inputActions = new CamMoveAndClick_Actions();
        m_playerEntity = GetComponent<EntityPlayer>();
    }

    private void OnEnable()
    {
        m_inputActions.Enable();
        m_inputActions.ClickActions.Click.performed += ClickPerformed;
    }
    private void OnDisable()
    {
        m_inputActions.Disable();
        m_inputActions.ClickActions.Click.performed -= ClickPerformed;
    }
    private void ClickPerformed(InputAction.CallbackContext context)
    {
        m_screenPos = m_inputActions.ClickActions.PointerPosition.ReadValue<Vector2>();
        m_screenClicked = true;
    }
    private void Update()
    {
        if (m_screenClicked)
        {
            m_screenClicked = false;
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI angeklickt");
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current) { position = m_screenPos };
                m_raycastResults.Clear();
                EventSystem.current.RaycastAll(pointerEventData, m_raycastResults);
                if (m_raycastResults.Count > 0 && m_raycastResults[0].gameObject.CompareTag("MenuBG"))//!!!!!!!!!!!!!!!!!!!!!MenuBG Tag setzen!!!!!!!!!!!!!
                {
                    UIManager.Instance.CloseAllMenues();
                    Debug.Log("UI Background angeklickt");
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(m_screenPos);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_clickableLayers))
                {
                    if (hit.collider.gameObject.CompareTag("wall")) return;
                    if (hit.collider.gameObject.TryGetComponent(out EntityEnemy ee))
                    {
                        UIManager.Instance.HandleEnemyClicked(ee.gameObject);
                    }
                    else
                    {
                        Vector3 clickPosition = new Vector3(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));//war Round()
                        Debug.Log(clickPosition + " angeklickt");
                        if (GraphManager.Instance.CanMoveToPosition(clickPosition))//out Node wär cool aber weiß nicht wie das programmiert wird
                        {
                            Debug.Log("Naechste Node dazu: " + GraphManager.Instance.FindClosestNode(clickPosition).Position);
                            LevelManager.Instance.CalculateEntityMovePath(m_playerEntity, GraphManager.Instance.FindClosestNode(clickPosition));
                        }
                    }
                }
            }
        }
    }
}
