using Ivankarez.AIFR.Common.Utils;
using TMPro;
using UnityEngine;

namespace Ivankarez.AIFR.UI.Components
{
    [ExecuteInEditMode]
    public class DataView : MonoBehaviour
    {
        [SerializeField] private string dataName = "name";
        [SerializeField] private string value = "100";
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text valueText;

        public string DataName
        {
            get { return dataName; }
            set
            {
                dataName = value;
                UpdateLabels();
            }
        }

        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                UpdateLabels();
            }
        }

        private void Awake()
        {
            Check.ArgumentNotNull(nameText, nameof(nameText));
            Check.ArgumentNotNull(valueText, nameof(valueText));
            UpdateLabels();
        }

        public void UpdateLabels()
        {
            nameText.text = $"{dataName}:";
            valueText.text = value ?? "-";
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (nameText.text != dataName || valueText.text != value)
            {
                UpdateLabels();
            }
        }
#endif
    }
}
