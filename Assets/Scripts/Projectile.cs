using UnityEngine;

public class Projectile : Bubble
{
    public float m_ChargeSpeed = .01f;
    public float m_LifeTime = 5f;
    public float m_Drag = .5f;
    public float m_ChargeSizeMult = .005f;
    public bool IsAttached {  get; set; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAttached && GameManager.Instance.m_ChargeMode) {
            m_Size += m_ChargeSpeed;
            Player.Instance.AddSize(-m_Size * m_ChargeSizeMult);
            transform.localScale = new Vector3(m_Size, m_Size, m_Size);
            transform.localPosition += new Vector3(m_ChargeSpeed/2f, 0,0);
        }
        else if (IsAttached) { 
            Player.Instance.AddSize(-m_Size * .001f);
        }
        m_LifeTime -= Time.deltaTime;
        if (m_LifeTime < 0) {
            Pop();
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("oui");
    }
}
