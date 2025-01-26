using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    Rigidbody2D m_rb;
    Vector3 m_MouseDir;
    Camera m_camera;
    Projectile m_Bubble;
    float m_ShootTime = .1f;
    bool m_IsShooting = false;
    Animator m_Animator;

    public GameObject m_BubblePrefab;
    public float m_ShotSpeed = 15f;
    public float m_FireRate = 1f;
    public float m_KnockBack = 1f;
    public float m_ChargeKnockBack = 15f;
    public float m_Speed = 1f;
    public float m_SizeMin = 0.21f;
    public float m_SizeMax = 0.38f;
    public float m_AngleMax = 1f;
    public bool m_Shrink = true;
    public float m_ShrinkFactor = 1f;
    public float m_GrowFactor = 1f;
    public float m_DeathScale = .5f;
    public bool m_ChargeMode = false;

    //sound variable:
    AudioSource m_DieSound;
    AudioSource m_CreateBigBubbleSound;
    public AudioSource[] m_SoundTab;

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
        m_Animator = GetComponentInChildren<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_DieSound = GetComponent<AudioSource>();
        m_SoundTab = GetComponentsInChildren<AudioSource>();
        m_CreateBigBubbleSound = GetComponent<AudioSource>();
        m_camera = Camera.main;
    }
    private void Update()
    {
        //Get mouse position relative to gameObject
        m_MouseDir = m_camera.ScreenToWorldPoint(Input.mousePosition);
        m_MouseDir -= transform.position;
        float horizontalRight = Input.GetAxis("HorizontalRight");
        float verticalRight = Input.GetAxis("VerticalRight");
        //If there is a right joystick input, the gameObject direction becomes this one instead
        if (horizontalRight != 0 || verticalRight != 0) {
            m_MouseDir = new Vector3(-horizontalRight, -verticalRight, 0);
        }
        Orientate(m_MouseDir);

        // Start shooting (mitraillette)
        if (!m_IsShooting && (Input.GetMouseButtonDown(0) || Input.GetAxis("Mitraillette") > 0)) {// || Input.GetAxis("HorizontalRight") > 0 || Input.GetAxis("VerticalRight") > 0) {
            m_IsShooting = true;
            m_ChargeMode = false;
            StartShoot();
        }
        // Stop shooting (mitraillette)
        if (!m_ChargeMode && m_IsShooting && (Input.GetMouseButtonUp(0) || Input.GetAxis("Mitraillette") == 0) && !Input.GetMouseButton(0) && !Input.GetMouseButton(1)) {// || Input.GetAxis("HorizontalRight") == 0 || Input.GetAxis
            m_IsShooting = false;
            StopShoot();
        }
        // Start shooting (charged)
        if (!m_IsShooting && (Input.GetMouseButtonDown(1) || Input.GetAxis("Charge") > 0)) {
            m_ChargeMode = true;
            m_IsShooting = true;
            StartShoot();
        }
        // Stop shooting (charged)
        if (m_ChargeMode && m_IsShooting && (Input.GetMouseButtonUp(1) || Input.GetAxis("Charge") ==  0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(0)){
            //m_ChargeMode = false;
            m_IsShooting = false;
            StopShoot();
        }

        if (!m_ChargeMode && m_IsShooting && Time.time - m_ShootTime > 1/m_FireRate) {
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

        // Spawn the bubble
        GameObject bubble;
        if (m_ChargeMode) {
            bubble = Instantiate(m_BubblePrefab, transform);
            bubble.transform.localPosition = new Vector3(.5f, 0, 0);
            }
        else
            bubble = Instantiate(m_BubblePrefab, transform.position, Quaternion.identity);

        // Disable physics so that it stays in front
        bubble.GetComponent<Rigidbody2D>().simulated = false;

        // Keep a ref to the bubble
        m_Bubble = bubble.GetComponent<Projectile>();

        //Set a random size for the bubble
        float m_RandomSize = Random.Range(m_SizeMin, m_SizeMax);
        m_Bubble.m_Size = m_RandomSize;
        m_Bubble.SetUpScale();

        // Let the bubble know it's attached
        m_Bubble.m_IsAttached = true;
    }

    void StopShoot()
    {
        if (m_Bubble == null) {
            Debug.Log("no bubble !");
        }


        Vector2 direction2 = new Vector2(m_MouseDir.x, m_MouseDir.y);
        direction2.Normalize();
        Vector3 direction = new Vector3(direction2.x, direction2.y, 0);

        // Activate physics simulation, give it a velocity
        Rigidbody2D bubble_rb = m_Bubble.GetComponent<Rigidbody2D>();
        bubble_rb.simulated = true;

        // Knockback
        float mult = m_ChargeMode ? m_ChargeKnockBack : 1;
        //Debug.Log("strength : " + m_Bubble.m_Size * m_KnockBack * mult + ", mult : " + mult);
        m_rb.AddForce(-direction * m_KnockBack * m_Bubble.m_Size * mult);

        // Random variation in bubble angle
        float randAngle = Random.Range(-m_AngleMax, m_AngleMax);
        Quaternion rotation = Quaternion.Euler(0,0,randAngle);
        direction = rotation * direction;

        // Launch bubble
        Vector3 current_velocity = new Vector3(m_rb.velocity.x, m_rb.velocity.y, 0);
        bubble_rb.velocity = current_velocity + direction * m_ShotSpeed;

        if (!m_ChargeMode) {
            AddSize(-.05f);
            m_CreateBigBubbleSound.Play();
        }
        int RandomValue = Random.Range(1,6);
        m_SoundTab[RandomValue].Play();
        // Let go of the bubble
        m_Bubble.transform.SetParent(null);
        m_Bubble.m_IsAttached = false;
        m_Bubble = null;

        // Animate Player
        m_Animator.SetTrigger("TrShoot");

        //m_ChargeMode = false;
    }

    // Adds size to the bubble
    public void AddSize(float size)
    {
        if (m_Shrink == false) return;
        size = size > 0 ? size * m_GrowFactor : size * m_ShrinkFactor;
        transform.localScale += new Vector3(size, size, size);
        if (transform.localScale.x <= m_DeathScale) { Die(); }
    }

    public void Die()
    {
        m_Animator.SetTrigger("TrPop");
        m_DieSound.Play();
        GameManager.Instance.ChangeState(GameState.GameOver);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_Animator.SetTrigger("TrBounce");
    }
}
