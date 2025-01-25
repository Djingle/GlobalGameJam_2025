using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    public float m_Size = .4f;
    public float m_SizeMult = .5f;
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
        if (m_IsAttached || other.gameObject.GetComponent<PlayerTrigger>() == null) {
            Debug.Log("return");
            return;
        }

        else {
            Debug.Log("pick up");
            PickUp();
        }
    }
}
