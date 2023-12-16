using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingBeetweenScenes : MonoBehaviour
{
    public void LoadScene(string name_scene) {
        SceneManager.LoadScene(name_scene);
    }
}
