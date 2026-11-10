using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector3 newPosition;
    private Vector3 speed = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)//lieber mit input action
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit))//�ber graph manager checken ob man da �berhaupt hin kann oder ob's ein gegner ist per Layermask
            {
                newPosition = new Vector3(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));//war Round()
                Debug.Log(newPosition);
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position,newPosition, ref speed, 0.3f);

    }
}
