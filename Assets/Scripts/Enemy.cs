using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float m_Speed = 1f;
    public bool m_Flip = false;

    bool m_Agro;
    Vector3 m_PlayerDir;
    Animator m_Animator;
    Vector3 m_SpawnPos;
    Quaternion m_SpawnRot;

	private void Awake() {
		m_Animator = GetComponent<Animator>();
        m_SpawnPos = transform.position;
        m_SpawnRot = transform.rotation;

        GameManager.StateChanged += Respawn;
	}

	private void OnDestroy() {
		GameManager.StateChanged -= Respawn;
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
        float offset = m_Flip ? rotZ + 180 : rotZ;
        transform.rotation = Quaternion.Euler(0, 0, offset);
    }

    // Set m_Agro and triggers agro animation
    public void Agro(bool agro)
    {
        if (agro) {
            m_Agro = true;
            if (m_Animator == null) return;
            m_Animator.SetTrigger("TrAgro");
        }
        else {
            m_Agro = false;
			if (m_Animator == null) return;
            m_Animator.SetTrigger("TrDesagro");
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null) return;
        Player.Instance.Die();
    }

    private void Respawn(GameState state) {
        if (state != GameState.GameOver) return;
        transform.position = m_SpawnPos;
        transform.rotation = m_SpawnRot;
        Agro(false);
    }
}
