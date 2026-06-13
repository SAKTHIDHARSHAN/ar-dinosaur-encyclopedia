using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementManager : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    public int currentPrefabIndex = 0;
    public TMP_Text infoText;

    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(
                    objectPrefabs[currentPrefabIndex],
                    hitPose.position + new Vector3(0, 0.1f, 0),
                    Quaternion.Euler(0, 90, 0)
                );
                DinosaurData data = spawnedObject.GetComponent<DinosaurData>();
                Debug.Log("Dinosaur found: " + data.dinosaurName);

                infoText.text =
                    "Name: " + data.dinosaurName +
                    "\nPeriod: " + data.period +
                    "\nDiet: " + data.diet+
                    "\nFact: " + data.fact;
            }
        }
    }

    public void ResetPlacement()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }

    public void SelectVelociraptor()
    {
        currentPrefabIndex = 0;
    }

    public void SelectStegosaurus()
    {
        currentPrefabIndex = 1;
    }

    public void SelectPachycephalosaurus()
    {
        currentPrefabIndex = 2;
    }
}