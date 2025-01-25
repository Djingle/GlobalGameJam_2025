using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    protected float m_Size = .4f;
    protected float m_SizeMult = 1f;
    public bool m_IsAttached;

    public void PickUp()
    {
        Player.Instance.AddSize(m_Size * m_SizeMult);
        Pop();
    }

    protected void Pop()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Trigger");
        if (m_IsAttached || other.gameObject.GetComponent<Player>() == null) {
            Debug.Log("return");
            return;
        }

        else {
            Debug.Log("pick up");
            PickUp();
        }
    }
}
