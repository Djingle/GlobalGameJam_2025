using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ScriptAudio : MonoBehaviour
{
    public AudioSource[] m_Tab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        m_Tab = GetComponents<AudioSource>();
        if (other.GetComponent<Player>() != null)
        {
            foreach (AudioSource a in m_Tab)
            {
                a.Play();
            }
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            foreach (AudioSource a in m_Tab)
            {
                a.Stop();
            }
        }
    }
}
