using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D m_rb;
    Vector3 m_MouseDir;
    Camera m_camera;
    Bubble m_Bubble;

    public GameObject m_BubblePrefab;
    public float m_ShotSpeed = 1f;
    public float m_KnockBack = 1f;

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
            m_Bubble = StartShoot();
        }
        if (Input.GetMouseButtonUp(0)) {
            StopShoot();
        }
    }

    void Orientate(Vector3 target)
    {
        float rotZ = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }

    Bubble StartShoot()
    {
        GameObject bubble = Instantiate(m_BubblePrefab, transform);
        bubble.transform.localPosition = new Vector3(1,0,0);
        bubble.GetComponent<Rigidbody2D>().simulated = false;
        //AddSize(-.1f);
        return bubble.GetComponent<Bubble>();
    }

    void StopShoot()
    {
        if (m_Bubble == null) {
            Debug.Log("no bubble !");
        }
        Rigidbody2D bubble_rb = m_Bubble.GetComponent<Rigidbody2D>();
        bubble_rb.simulated = true;
        bubble_rb.velocity = m_MouseDir * m_ShotSpeed;
        m_rb.AddForce(-m_MouseDir * m_KnockBack);
        m_Bubble.transform.SetParent(null);
    }
}
