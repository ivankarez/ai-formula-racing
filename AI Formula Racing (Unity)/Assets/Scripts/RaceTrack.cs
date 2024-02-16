using Ivankarez.AIFR.Common.Utils;
using Ivankarez.RacetrackGenerator;
using UnityEngine;

namespace Ivankarez.AIFR
{
    public class RaceTrack : MonoBehaviour
    {
        public TrackData RaceTrackData => raceTrackData;
        public Transform StartPoint => startPoint;

        [SerializeField] private TrackData raceTrackData;
        [SerializeField] private Transform startPoint;

        private void Awake()
        {
            Check.ArgumentNotNull(raceTrackData, nameof(raceTrackData));
            Check.ArgumentNotNull(startPoint, nameof(startPoint));
        }
    }
}
