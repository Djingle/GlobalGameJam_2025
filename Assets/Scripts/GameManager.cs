using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;
public class GameManager : MonoBehaviour {

    public static Action<GameState> StateChanged;
    // The GameManager singleton instance : the GameManager Script can be accessed anywhere with : GameManager.Instance
    public static GameManager Instance { get; private set; }
    // The state the game is currently in. It should only be updated by ChangeState
    public GameState State { get; private set; }
    public Vector3 m_SpawnLocation = Vector3.zero;
    public GameObject m_PlayerPrefab;
    public GameObject m_PickUpPrefab;
    public Transform m_LayerObject;
    public float m_SpawnSpeed = 1f;

    Coroutine m_SpawnCoroutine = null;
    List<PickUp> m_PickUps;

    private void Awake()
    {
        // Keep the GameManager when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Singleton checks
        if (Instance == null) { // If there is no instance of GameManager yet, then this one becomes the only instance
            Instance = this;
        } else {                // If a GameManager instance already exists, destroy the new one
            Debug.LogWarning("GameManager Instance already exists, destroying the duplicate");
            Destroy(gameObject);
            return;
        }

        /*foreach (PickUpBubble b in m_PickUps) {
            Debug.Log("b pos : " + b.transform.position);
        }*/

        m_PickUps = new List<PickUp>();
    }

    private void Start()
    {
        SavePickUps();
        ChangeState(GameState.Menu);
    }

    public void ChangeState(GameState newState)
    {
        // Change the state variable
        State = newState;
        // Run some code depending on the new state
        switch (newState) {
            case GameState.Playing:
                break;
            case GameState.Menu:
                break;
            case GameState.GameOver:
                Debug.Log("GAME OVER");
                if (m_SpawnCoroutine != null) return;
				Debug.Log("START COROUTINE");
				m_SpawnCoroutine = StartCoroutine(DieAndRespawn());
                break;
            case GameState.Credits:
                break;
        }

        // Send the event to every listening script
        StateChanged?.Invoke(newState);
    }

	// Save location of every Bubble in the level to spawn them back when player dies
	private void SavePickUps() {
        PickUpBubble[] pickUps = FindObjectsByType<PickUpBubble>(FindObjectsSortMode.InstanceID);
        foreach (PickUpBubble pickup in pickUps) {
            PickUp newPickUp = new(pickup.transform.position, pickup.m_Size, pickup.gameObject);
            m_PickUps.Add(newPickUp);
        }
	}

    private void Spawn()
    {
		Instantiate(m_PlayerPrefab, m_SpawnLocation, Quaternion.identity);
        Player.Instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, m_SpawnSpeed));
	}

    private void SpawnPickUps()
    {
        Debug.Log("Spawning Pick Ups");
        foreach(PickUp bubble in m_PickUps) {
            if (bubble.instance != null) continue;

            Bubble newBubble= Instantiate(m_PickUpPrefab, bubble.position, Quaternion.identity, m_LayerObject).GetComponent<Bubble>();

            newBubble.m_Size = bubble.size;
            newBubble.SetUpScale();
        }
    }

    // Coroutine that waits for 1 sec, destroys the player
    IEnumerator DieAndRespawn()
    {
        yield return new WaitForSeconds(1);
		Debug.Log("----DIE----");
		Destroy(Player.Instance.gameObject);
        yield return new WaitForSeconds(.3f);
		Debug.Log("---SPAWN---");
		Spawn();

		Debug.Log("---BUBBLE--");
		SpawnPickUps();
        Cameraman.Instance.SetTarget();

        m_SpawnCoroutine = null;
    }
}




// Different states the game can be in. Can be accessed with : GameState.exampleState
public enum GameState
{
    Playing,
    Menu,
    GameOver,
    Credits
}

public class PickUp 
{
    public Vector3 position;
    public float size;
    public GameObject instance;

	public PickUp(Vector3 position, float size, GameObject instance) {
        this.instance = instance;
        this.position = position;
        this.size = size;
	}
}