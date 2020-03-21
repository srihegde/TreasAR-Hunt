using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualBtnTrigger : MonoBehaviour, IVirtualButtonEventHandler
{

    public GameObject vBtn;
    public GameObject nextHint;
    public string nextHintName = "Hint 2";

    // Start is called before the first frame update
    void Start()
    {
        vBtn = GameObject.Find("NextBtn_1");
        vBtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);

        nextHint = GameObject.Find(nextHintName);
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        vBtn.SetActive(false);
        nextHint.SetActive(true);
        //ActivateTarget(datasetName);
        Debug.Log("Btn 1 Pressed!");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("Btn 1 Released!");
    }

    //This function will load and activate the designated dataset. It will not de-activate
    //anything, so be sure no other Model Target datasets are active to avoid issues.
    public void ActivateTarget(string loadThisDataset)
    {
        TrackerManager trackerManager = (TrackerManager)TrackerManager.Instance;
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        //Stop the tracker.
        objectTracker.Stop();

        //Create a new dataset object.
        DataSet dataset = objectTracker.CreateDataSet();

        //Load and activate the dataset if it exists.
        if (DataSet.Exists(loadThisDataset))
        {
            dataset.Load(loadThisDataset);
            objectTracker.ActivateDataSet(dataset);
        }

        //Start the object tracker.
        objectTracker.Start();
    }
}
