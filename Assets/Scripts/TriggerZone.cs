using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public Enemy[] m_Enemies;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Player>() == null) return;
        foreach (Enemy enemy in m_Enemies) {
            enemy.Agro(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.GetComponent<Player>() == null) return;
        foreach (Enemy enemy in m_Enemies) {
            enemy.Agro(false);
        }
    }
}
