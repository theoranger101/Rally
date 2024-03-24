using UnityEngine;

namespace Utility
{
    [CreateAssetMenu(menuName = "General Settings")]
    public class GeneralSettings : ScriptableObject
    {
        private static GeneralSettings _GeneralSettings;

        private static GeneralSettings generalSettings
        {
            get
            {
                if (!_GeneralSettings)
                {
                    _GeneralSettings = Resources.Load<GeneralSettings>($"GeneralSettings");
                }

                return _GeneralSettings;
            }
        }

        public static GeneralSettings Get()
        {
            return generalSettings;
        }
        
        public float CarMoveSpeed = 5f;
        public float MaxCarSpeed = 20f;

        public float LeftBoundary = -2f;
        public float RightBoundary = 2f; 
        
        public float PowerupMultiplier = 1.5f;
        public float PowerupTimer = 5f;

        public bool UseOSC;
    }
}