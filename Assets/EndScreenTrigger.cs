using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null) return;
        Debug.Log("OUAAAAAAIS");
        SceneManager.LoadSceneAsync("Win Scene");
        //Player.Instance.Die();
    }
}
