using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSCENE : MonoBehaviour
{
 public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Tutoriel_SCENE");
    }
 
}
