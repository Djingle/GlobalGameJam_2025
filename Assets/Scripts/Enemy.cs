using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float m_Speed = 1f;

    bool m_Aggro;
    Collider2D m_Collider;

    
    private void Update()
    {
        if (m_Aggro)
            transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, m_Speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() == null) return;

        m_Aggro = true;
    }
}
