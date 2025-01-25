using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private float m_Size;
    public float m_ChargeSpeed = .01f;
    public bool IsAttached {  get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("salut");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttached && GameManager.Instance.ChargeMode) {
            transform.localScale += new Vector3(m_ChargeSpeed, m_ChargeSpeed, m_ChargeSpeed);
            transform.localPosition += new Vector3(m_ChargeSpeed/2f, 0,0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("oui");
    }
}
