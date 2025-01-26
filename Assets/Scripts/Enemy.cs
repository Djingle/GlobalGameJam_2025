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
        if (!m_Aggro) return;
        
        transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, m_Speed);
        Orientate(Player.Instance.transform.position);
    }

    void Orientate(Vector3 target)
    {
        float rotZ = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ + 180);
    }

    public void Aggro()
    {
        m_Aggro = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null) return;
        Player.Instance.Die();
    }
}
