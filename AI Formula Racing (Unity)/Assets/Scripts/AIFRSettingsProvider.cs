using UnityEngine;

namespace Ivankarez.AIFR
{
    public class AIFRSettingsProvider : MonoBehaviour
    {
        private AIFRSettings settings;

        public AIFRSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = Resources.Load<AIFRSettings>("AIFRSettings");
                    if (settings == null)
                    {
                        Debug.LogError("AIFRSettings instance not found in Resources folder");
                    }
                }

                return settings;
            }
        }
    }
}