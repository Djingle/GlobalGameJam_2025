using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D m_rb;
    Vector3 m_mousePos;
    Camera m_camera;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_camera = Camera.main;
    }
    private void Update()
    {
        m_mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        Orientate(m_mousePos);
    }

    void Orientate(Vector3 target)
    {
        Vector3 rotation = target - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }

    void Shoot(Vector2 direction)
    {
        
    }
}
