using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    public static Cameraman Instance { get; private set; }
    private Transform target;
    private void Awake()
    {
        // Keep the GameManager when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null) { // If there is no instance of GameManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a GameManager instance already exists, destroy the new one
            Debug.LogWarning("Cameraman Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        target = Player.Instance.transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    public void SetTarget()
    {
        target = Player.Instance.transform;
    }
}
