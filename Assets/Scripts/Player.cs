using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    Rigidbody2D m_rb;
    Vector3 m_MouseDir;
    Camera m_camera;
    Bubble m_Bubble;
    float m_ShootTime = 0f;
    bool m_IsShooting = false;

    public GameObject m_BubblePrefab;
    public float m_ShotSpeed = 1f;
    public float m_FireRate = 1f;
    public float m_KnockBack = 1f;
    public float m_Speed = 1f;

    private void Awake()
    {
        // Keep the GameManager when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null) { // If there is no instance of GameManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a GameManager instance already exists, destroy the new one
            Debug.LogWarning("Player Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_camera = Camera.main;
    }
    private void Update()
    {
        m_MouseDir = m_camera.ScreenToWorldPoint(Input.mousePosition);
        m_MouseDir -= transform.position;
        //float horizontalRight = Input.GetAxis("HorizontalRight");
        //float verticalRight = Input.GetAxis("VerticalRight");
        //if (horizontalRight > -1 && horizontalRight < 1 || verticalRight > -1 && verticalRight <1) {
        //    m_MouseDir = new Vector3(horizontalRight, verticalRight, 0);
        //}
        Orientate(m_MouseDir);



        if (Input.GetMouseButtonDown(0)) {// || Input.GetAxis("HorizontalRight") > 0 || Input.GetAxis("VerticalRight") > 0) {
            m_IsShooting = true;
            StartShoot();
        }
        if (Input.GetMouseButtonUp(0)) {// || Input.GetAxis("HorizontalRight") == 0 || Input.GetAxis("VerticalRight") == 0) {
            m_IsShooting = false;
            StopShoot();
        }

        if (!GameManager.Instance.m_ChargeMode && m_IsShooting && Time.time - m_ShootTime > 1/m_FireRate) {
            StopShoot();
            StartShoot();
        }
    }

    private void FixedUpdate()
    {
        Vector2 force = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f) * m_Speed;
        GetComponent<Rigidbody2D>().AddForce(force);
    }

    void Orientate(Vector3 target)
    {
        float rotZ = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,rotZ);
    }

    void StartShoot()
    {
        if (m_Bubble != null) {
            Debug.LogWarning("There is already a bubble !");
            return;
        }
        m_ShootTime = Time.time;
        GameObject bubble = Instantiate(m_BubblePrefab, transform);
        bubble.transform.localPosition = new Vector3(.7f,0,0);
        bubble.transform.localScale = new Vector3(.4f, .4f, .4f);
        bubble.GetComponent<Rigidbody2D>().simulated = false;
        m_Bubble = bubble.GetComponent<Bubble>();
        m_Bubble.IsAttached = true;
        //AddSize(-.1f);
    }

    void StopShoot()
    {
        if (m_Bubble == null) {
            Debug.Log("no bubble !");
        }
        Rigidbody2D bubble_rb = m_Bubble.GetComponent<Rigidbody2D>();
        bubble_rb.simulated = true;
        bubble_rb.velocity = m_MouseDir * m_ShotSpeed;
        m_rb.AddForce(-m_MouseDir * m_KnockBack);
        m_Bubble.transform.SetParent(null);
        m_Bubble.IsAttached = false;
        m_Bubble = null;
    }
}
