using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator m_Animator;
    private bool m_IsShooting = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Animator != null)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetAxis("RTrigger") > 0 && !m_IsShooting)
            {
                m_Animator.SetTrigger("TrShoot");
                m_IsShooting = true;
            }
            if (Input.GetMouseButtonUp(0) || Input.GetAxis("RTrigger") == 0 && !Input.GetMouseButton(0) && m_IsShooting)
            {
                m_Animator.SetTrigger("TrStopShoot");
                m_IsShooting = false;
            }
        }
    }
}
