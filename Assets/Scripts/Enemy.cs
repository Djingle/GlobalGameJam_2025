using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float m_Speed = 1f;

    bool m_Agro;
    Vector3 m_PlayerDir;
    Animator m_Animator;

	private void Awake() {
		m_Animator = GetComponent<Animator>();
	}


	private void FixedUpdate()
    {
        if (!m_Agro) return;

        transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, m_Speed);
        m_PlayerDir = transform.position - Player.Instance.transform.position;
        Orientate(m_PlayerDir);
    }

    void Orientate(Vector3 target)
    {
        float rotZ = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    // Set m_Agro and triggers agro animation
    public void Agro(bool agro)
    {
        if (agro) {
            m_Agro = true;
            m_Animator.SetTrigger("TrAgro");
        }
        else {
            m_Agro = false;
            m_Animator.SetTrigger("TrDesagro");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null) return;
        Player.Instance.Die();
    }
}
