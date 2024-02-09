using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Ivankarez.AIFR
{
    public class TrackSceneLoader : MonoBehaviour
    {
        [SerializeField] private RaceTrackName trackToLoad;
        public UnityEvent<RaceTrack> OnTrackLoaded;

        public RaceTrack LoadedRaceTrack { get; private set; }

        private void Start()
        {
            StartCoroutine(LoadTrackScene());
        }

        private IEnumerator LoadTrackScene()
        {
            var sceneName = MapToSceneName(trackToLoad);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            LoadedRaceTrack = FindFirstObjectByType<RaceTrack>();
            if (LoadedRaceTrack == null)
            {
                throw new System.Exception($"No RaceTrack instance found in scene {sceneName}");
            }
            OnTrackLoaded?.Invoke(LoadedRaceTrack);
        }

        private string MapToSceneName(RaceTrackName trackName)
        {
            if (trackName == RaceTrackName.Hungaroring)
            {
                return "Hungaroring Race Track Scene";
            }
            if (trackName == RaceTrackName.CircuitDeBarcelonaCatalunya)
            {
                return "Circuit de Barcelona-Catalunya Race Track Scene";
            }

            throw new System.Exception($"No scene name provided for track {trackName}");
        }
    }

    public enum RaceTrackName
    {
        Hungaroring, CircuitDeBarcelonaCatalunya
    }
}
