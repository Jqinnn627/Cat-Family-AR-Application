using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleMarker : MonoBehaviour
{   
    public ARTrackedImageManager imageManager;

    [System.Serializable]
    public class CatData
    {
        public string markerName;
        public GameObject prefab;
    }

    public List<CatData> meow = new List<CatData>();
    private GameObject currCat = null;
    private string currMarker = "";
    private bool canSpawn = true;

    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImageChanged;
    }
    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (!canSpawn) return;

        foreach (var trackedImage in args.added)
        {
            SpawnCat(trackedImage);
        }

        foreach (var trackedImage in args.updated)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                SpawnCat(trackedImage);
            }
        }

    }
    void SpawnCat(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;

        if( currCat != null )
        {
            return;
        }

        foreach (var animal in meow)
        {
            if (animal.markerName == name && animal.prefab != null)
            {
                GameObject newAnimal = Instantiate(animal.prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                newAnimal.transform.localPosition = new Vector3(0, 0, 0.05f);
                newAnimal.transform.SetParent(trackedImage.transform);
                currCat = newAnimal;
                canSpawn = false;
                break;
            }
        }
    }

    public void ClearObject()
    {
        if (currCat != null)
        {
            Destroy(currCat);
            currCat = null;
        }
        currMarker = "";
        canSpawn = true;
    }
}
