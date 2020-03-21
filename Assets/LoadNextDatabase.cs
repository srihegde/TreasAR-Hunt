using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Vuforia;

public class LoadNextDatabase : MonoBehaviour
{
    public string dataset;
    private IEnumerable<string> datasets;
    private int DSCounter;

    // Use this for initialization
    void Start()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    private void OnVuforiaStarted()
    {
        // Get level-sorted datasets
        SwitchTargetByName(dataset);
        //ActivateTarget(datasetToLoad);
        return;
    }

    private IEnumerable<string> GetAllDatasets()
    {
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        IEnumerable<DataSet> datasets = objectTracker.GetDataSets();

        List<string> datasetsName  = new List<string>();
        foreach (DataSet ds in datasets)
        {
            datasetsName.Add(Path.GetFileNameWithoutExtension(ds.Path));
        }

        // Sorting the datasets names
        datasetsName = datasetsName.OrderBy(q => q).ToList();

        return datasetsName;
    }

    void Update()
    {
        //VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnVuforiaUpdated);
    }

    private void OnVuforiaUpdated()
    {
        String marking = "Hint " + (DSCounter+1).ToString();
        bool isHintExplored = IsMarkingTracked(marking);
        if (isHintExplored)
        {
            DSCounter++;
            SwitchTargetByName(datasets.ElementAt(DSCounter));
        }

    }

    private bool IsMarkingTracked(string marking)
    {
        GameObject target1 = GameObject.Find(marking);
        TrackableBehaviour trackable = target1.GetComponent<TrackableBehaviour>();
        TrackableBehaviour.Status status = trackable.CurrentStatus;
        return status == TrackableBehaviour.Status.TRACKED;
    }

    //This function will de-activate all current datasets and activate the designated dataset.
    //It is assumed the dataset being activated has already been loaded (either through code
    //elsewhere or via the Vuforia Configuration).
    public void SwitchTargetByName(string activateThisDataset)
    {
        TrackerManager trackerManager = (TrackerManager)TrackerManager.Instance;
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        IEnumerable<DataSet> datasets = objectTracker.GetDataSets();
        IEnumerable<DataSet> activeDataSets = objectTracker.GetActiveDataSets();
        List<DataSet> activeDataSetsToBeRemoved = activeDataSets.ToList();

        //Loop through all the active datasets and deactivate them.
        foreach (DataSet ads in activeDataSetsToBeRemoved)
        {
            objectTracker.DeactivateDataSet(ads);
        }

        //Swapping of the datasets should not be done while the ObjectTracker is working at the same time.
        //So, Stop the tracker first.
        objectTracker.Stop();

        //Then, look up the new dataset and if one exists, activate it.
        foreach (DataSet ds in datasets)
        {
            if (ds.Path.Contains(activateThisDataset))
            {
                objectTracker.ActivateDataSet(ds);
            }
        }

        //Finally, start the object tracker.
        objectTracker.Start();
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
