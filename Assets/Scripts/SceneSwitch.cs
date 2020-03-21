using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public int sceneNo;
    public void SceneSwitcher()
    {
        SceneManager.LoadScene(sceneNo);
    }
}
