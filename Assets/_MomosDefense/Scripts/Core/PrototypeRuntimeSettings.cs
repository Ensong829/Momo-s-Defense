using UnityEngine;

namespace MomosDefense.Core
{
    public sealed class PrototypeRuntimeSettings : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 120;
        [SerializeField] private int qualityLevel = 1;
        [SerializeField] private int vSyncCount;
        [SerializeField] private float fixedDeltaTime = 1f / 60f;
        [SerializeField] private float maximumDeltaTime = 0.05f;
        [SerializeField] private bool runInBackground = true;

        private void Awake()
        {
            Apply();
        }

        private void OnValidate()
        {
            targetFrameRate = Mathf.Max(30, targetFrameRate);
            qualityLevel = Mathf.Max(0, qualityLevel);
            fixedDeltaTime = Mathf.Clamp(fixedDeltaTime, 1f / 240f, 1f / 30f);
            maximumDeltaTime = Mathf.Clamp(maximumDeltaTime, fixedDeltaTime, 0.2f);
        }

        private void Apply()
        {
            if (QualitySettings.names.Length > 0)
            {
                QualitySettings.SetQualityLevel(Mathf.Min(qualityLevel, QualitySettings.names.Length - 1), true);
            }

            QualitySettings.vSyncCount = Mathf.Max(0, vSyncCount);
            Application.targetFrameRate = targetFrameRate;
            Application.runInBackground = runInBackground;
            Time.fixedDeltaTime = fixedDeltaTime;
            Time.maximumDeltaTime = maximumDeltaTime;
        }
    }
}
