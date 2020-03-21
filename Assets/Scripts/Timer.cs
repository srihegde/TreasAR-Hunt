using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private const string SEPERATOR = "";

    private class SaveObject
    {
        public string sceneName;
        public float minutes;
        public float seconds;
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if ( currentScene == SceneManager.GetSceneByName("OpeningHint"))
        {
            GlobalCountDown.StartCountDown(TimeSpan.FromMinutes(30));
        }

        SaveTimeToFile(currentScene);

        //For testing
        //GlobalCountDown.StartCountDown(TimeSpan.FromSeconds(10));
    }

    private void SaveTimeToFile(Scene scene)
    {
        //string filename = Application.dataPath + "/timings.txt";

        //For mobile devices
        string filename = Application.persistentDataPath + "/timings.txt";

        //Data for saving
        SaveObject saveObject = new SaveObject
        {
            sceneName = scene.name,
            minutes = GlobalCountDown.TimeLeft.Minutes,
            seconds = GlobalCountDown.TimeLeft.Seconds
        };
        string jsonData = JsonUtility.ToJson(saveObject);
        File.AppendAllText(filename, jsonData + Environment.NewLine);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalCountDown.TimeLeft.TotalSeconds < 0)
        {
            SceneManager.LoadScene("GamerOver");
        }
        else
        {
            float minutes = GlobalCountDown.TimeLeft.Minutes;
            float seconds = GlobalCountDown.TimeLeft.Seconds;

            timerText.text = minutes.ToString() + ":" + seconds.ToString();
        }
    }
}
