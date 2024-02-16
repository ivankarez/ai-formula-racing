using Ivankarez.AIFR.Common.Utils;
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
                    Check.ArgumentNotNull(settings);
                }

                return settings;
            }
        }
    }
}