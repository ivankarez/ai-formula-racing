using Ivankarez.RacetrackGenerator;
using UnityEngine;

namespace Ivankarez.AIFR
{
    public class RaceTrack : MonoBehaviour
    {
        [SerializeField] private TrackData raceTrackData;
        [SerializeField] private Transform startPoint;

        public TrackData RaceTrackData => raceTrackData;
        public Transform StartPoint => startPoint;
    }
}
