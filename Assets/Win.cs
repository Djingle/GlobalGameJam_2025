using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
 public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Intro_SCENE");
    }
 
}
