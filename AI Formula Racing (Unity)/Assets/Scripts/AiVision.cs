using UnityEngine;

namespace Ivankarez.AIFR
{
    public class AiVision : MonoBehaviour
    {
        private RenderTexture renderTexture;

        [SerializeField] private Camera visionCamera;
        [SerializeField] private AIFRSettingsProvider settingsProvider;

        public float[] Values { get; private set; }
        public Texture2D Texture2d { get; private set; }

        private void Awake()
        {
            var settings = settingsProvider.Settings;
            var width = settings.CameraResolution;
            var height = settings.CameraResolution;
            renderTexture = new RenderTexture(width, height, 1);
            Texture2d = new Texture2D(renderTexture.width, renderTexture.height);
            visionCamera.targetTexture = renderTexture;
            Values = new float[renderTexture.width * renderTexture.height];
        }

        public void UpdateValues()
        {
            Update2DTexture();
            ReadValues();
        }

        private void Update2DTexture()
        {
            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;
            Texture2d.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            Texture2d.Apply();
            RenderTexture.active = activeRenderTexture;
        }

        private void ReadValues()
        {
            for (int x = 0; x < Texture2d.width; x++)
            {
                for (int y = 0; y < Texture2d.height; y++)
                {
                    var pixel = Texture2d.GetPixel(x, y);
                    var value = pixel.grayscale;
                    Values[x * Texture2d.height + y] = value;
                    Texture2d.SetPixel(x, y, new Color(value, value, value, 1f));
                }
            }
            Texture2d.Apply();
        }
    }
}
