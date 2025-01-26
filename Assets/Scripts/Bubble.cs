using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    public float m_Size = .4f;
    public bool m_IsAttached;

    protected Collider2D m_Collider;

    public void SetUpScale()
    {
        transform.localScale = new Vector3(m_Size, m_Size, m_Size);
    }

    public void PickUp()
    {
        Player.Instance.AddSize(m_Size);
        Pop();
    }

    protected void Pop()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_IsAttached || other.gameObject.GetComponent<PlayerTrigger>() == null) {
            return;
        }

        else {
            PickUp();
        }
    }
}
