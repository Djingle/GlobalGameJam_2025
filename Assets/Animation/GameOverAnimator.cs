using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverAnimator : MonoBehaviour
{
    private Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        GameManager.StateChanged += OnStateChanged;
    }

    void OnStateChanged(GameState gameState)
    {
        Debug.Log("Shrug");
        if (gameState == GameState.GameOver)
        {
            m_Animator.SetTrigger("TrGameOver");
        }
    }
}
