using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    protected float m_Size = .4f;
    protected float m_SizeMult = 1f;
    
    public void PickUp()
    {
        Player.Instance.AddSize(m_Size * m_SizeMult);
        Pop();
    }

    protected void Pop()
    {
        Destroy(gameObject);
    }
}
