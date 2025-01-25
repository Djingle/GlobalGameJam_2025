using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : Bubble
{
    public float m_ChargeSpeed = .01f;
    public float m_LifeTime = 5f;
    public float m_Drag = .5f;
    public float m_ChargeSizeMult = .005f;
    public float m_ColliderActivationTimer = 10f;


    // Start is called before the first frame update
    void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
        m_IsAttached = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Grow the bubble if charge mode is on and it's attached
        if (m_IsAttached && GameManager.Instance.m_ChargeMode) {
            m_Size += m_ChargeSpeed;
            Player.Instance.AddSize(-m_Size * m_ChargeSizeMult);
            transform.localScale = new Vector3(m_Size, m_Size, m_Size);
            transform.localPosition += new Vector3(m_ChargeSpeed/2f, 0,0);
            Debug.Log("oui");
        }


        if (!m_IsAttached && m_ColliderActivationTimer > 0) {
            m_ColliderActivationTimer -= Time.deltaTime;
            if (m_ColliderActivationTimer < 0) m_Collider.enabled = true;
        }

        m_LifeTime -= Time.deltaTime;
        if (m_LifeTime < 0) {
            Pop();
        }

    }
}
