using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class camMovement : MonoBehaviour
{
    public float angleHor = 0f;
    public float radius = 10f;
    public float rotation = 400f;
    public float angleVer;

    public Vector3 target;
    // Start is called once before the first exHorecution of Update after the MonoBehaviour is created
    void Start()
    {
        angleVer = Mathf.Asin(transform.position.y / radius) * Mathf.Rad2Deg;
        angleHor = Mathf.Atan2(transform.position.x, transform.position.z) * Mathf.Rad2Deg;
        target = new Vector3((PlayingFieldManager.instance.size * 12f) / 2f, 0f, (PlayingFieldManager.instance.size * 12f) / 2f) - new Vector3(0.5f, 0f, 0.5f);//- PlayingFieldManager.instance.offset; //target ist mitte vom spielfeld
    }

    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.aKey.isPressed)
        {
            angleHor += rotation * Time.deltaTime;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            angleHor -= rotation * Time.deltaTime;
        }

         


            if (Keyboard.current.wKey.isPressed)
            {
                if ((angleVer + rotation * Time.deltaTime) < 90)
                {
                    angleVer += rotation * Time.deltaTime;
                }
                
            }
            if (Keyboard.current.sKey.isPressed)
            {
                if (angleVer - rotation * Time.deltaTime > 45)
                {
                    angleVer -= rotation * Time.deltaTime;
                }
                
            }
        

        float x = radius * Mathf.Cos(angleVer * Mathf.Deg2Rad) * Mathf.Sin(angleHor * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angleVer * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(angleVer * Mathf.Deg2Rad) * Mathf.Cos(angleHor * Mathf.Deg2Rad);

        
        transform.position = new Vector3(x, y, z) + target;
        transform.LookAt(target);//Vector3.zero
    }
}
//es wär nice to have, wenn die Kamera bei einem steileren winkel auch weiter weg ist, oder FOV halt anpassen
//so dass man mehr sieht, wenn weiter weg