using Ivankarez.AIFR;
using Ivankarez.AIFR.Common.Utils;
using UnityEngine;

public class VehicleTestSceneManager : MonoBehaviour
{
    [SerializeField] private Transform vehicleTransform;

    private Transform startPoint;

    private void Start()
    {
        vehicleTransform.gameObject.SetActive(false);
    }

    public void OnRaceTrackLoaded(RaceTrack raceTrack)
    {
        Check.ArgumentNotNull(raceTrack, nameof(raceTrack));

        startPoint = raceTrack.StartPoint;
        vehicleTransform.gameObject.SetActive(true);
        ResetCar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCar();
        }
    }

    private void ResetCar()
    {
        vehicleTransform.position = startPoint.position;
        vehicleTransform.rotation = startPoint.rotation;
    }
}
