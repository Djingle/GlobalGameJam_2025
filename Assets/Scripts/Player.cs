using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject m_BubblePrefab;
    Rigidbody2D m_rb;
    Vector3 m_MouseDir;
    Camera m_camera;

    public float m_ShotSpeed = 1f;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_camera = Camera.main;
    }
    private void Update()
    {
        m_MouseDir = m_camera.ScreenToWorldPoint(Input.mousePosition);
        m_MouseDir -= transform.position;
        Orientate(m_MouseDir);
        if (Input.GetMouseButtonDown(0)) {
            Shoot();
            Debug.Log("shoot");
        }
    }

    void Orientate(Vector3 target)
    {
        float rotZ = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }

    void Shoot()
    {
        GameObject bubble = Instantiate(m_BubblePrefab, transform.position, Quaternion.identity);
        Rigidbody2D bubble_rb = bubble.GetComponent<Rigidbody2D>();
        bubble_rb.velocity = m_MouseDir * m_ShotSpeed;
        Debug.Log(m_MouseDir);
    }
}
