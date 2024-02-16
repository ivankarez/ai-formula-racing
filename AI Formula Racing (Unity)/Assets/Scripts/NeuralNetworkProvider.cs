using UnityEngine;
using Ivankarez.NeuralNetworks;
using Ivankarez.NeuralNetworks.Api;
using Ivankarez.AIFR.Common.Utils;

namespace Ivankarez.AIFR
{
    public class NeuralNetworkProvider : MonoBehaviour
    {
        [SerializeField] private AIFRSettingsProvider settingsProvider;

        private void Awake()
        {
            Check.ArgumentNotNull(settingsProvider, nameof(settingsProvider));
        }

        public LayeredNetworkModel CreateImageEmbeddingModel()
        {
            var settings = settingsProvider.Settings;
            var imageRes = settings.CameraResolution;
            return NN.Models.Layered(
                NN.Size.Of(imageRes, imageRes),
                NN.Layers.Conv2D(NN.Size.Of(3, 3)),
                NN.Layers.Conv2D(NN.Size.Of(3, 3)),
                NN.Layers.Pooling2D(NN.Size.Of(2, 2)),
                NN.Layers.Conv2D(NN.Size.Of(3, 3)),
                NN.Layers.Dense(settings.ImageEmbeddingVectorSize));
        }

        public LayeredNetworkModel CreateDrivingModel()
        {
            var settings = settingsProvider.Settings;
            var embeddingVectorSize = settings.ImageEmbeddingVectorSize;
            return NN.Models.Layered(
                NN.Size.Of(embeddingVectorSize),
                NN.Layers.GRU(NN.Size.Of(10)),
                NN.Layers.Dense(5),
                NN.Layers.Dense(2, activation: NN.Activations.ClampedLinear()));
        }
    }
}
