using UnityEngine;
using UnityEngine.XR.Content.Interaction;

namespace Settings
{
    public class LocomotionPreferencesManager
    {
        
        public void SavePreferences(LocomotionManager manager)
        {
            // Locomotion type
            PlayerPrefs.SetInt(LocomotionSettingsKey.LeftHandLocomotionType.ToString(), (int)manager.leftHandLocomotionType);
            PlayerPrefs.SetInt(LocomotionSettingsKey.RightHandLocomotionType.ToString(), (int)manager.rightHandLocomotionType);

            // Turn style preferences
            PlayerPrefs.SetInt(LocomotionSettingsKey.LeftHandTurnStyle.ToString(), (int)manager.leftHandTurnStyle);
            PlayerPrefs.SetInt(LocomotionSettingsKey.RightHandTurnStyle.ToString(), (int)manager.rightHandTurnStyle);
            
            PlayerPrefs.SetInt(LocomotionSettingsKey.EnableStrafe.ToString(), manager.dynamicMoveProvider.enableStrafe ? 1 : 0);
            PlayerPrefs.SetInt(LocomotionSettingsKey.EnableComfortMode.ToString(), manager.enableComfortMode ? 1 : 0);
            PlayerPrefs.SetInt(LocomotionSettingsKey.EnableTurnAround.ToString(), manager.snapTurnProvider.enableTurnAround ? 1 : 0);


            PlayerPrefs.Save();
        }
        
        public void LoadPreferences(LocomotionManager manager)
        {
            // Locomotion type
            manager.leftHandLocomotionType = (LocomotionManager.LocomotionType)PlayerPrefs.GetInt(LocomotionSettingsKey.LeftHandLocomotionType.ToString(), (int)manager.leftHandLocomotionType);
            manager.rightHandLocomotionType = (LocomotionManager.LocomotionType)PlayerPrefs.GetInt(LocomotionSettingsKey.RightHandLocomotionType.ToString(), (int)manager.rightHandLocomotionType);

            // Turn style preferences
            manager.leftHandTurnStyle = (LocomotionManager.TurnStyle)PlayerPrefs.GetInt(LocomotionSettingsKey.LeftHandTurnStyle.ToString(), (int)manager.leftHandTurnStyle);
            manager.rightHandTurnStyle = (LocomotionManager.TurnStyle)PlayerPrefs.GetInt(LocomotionSettingsKey.RightHandTurnStyle.ToString(), (int)manager.rightHandTurnStyle);
            
            manager.dynamicMoveProvider.enableStrafe = PlayerPrefs.GetInt(LocomotionSettingsKey.EnableStrafe.ToString(), manager.dynamicMoveProvider.enableStrafe ? 1 : 0) == 1;
            manager.enableComfortMode = PlayerPrefs.GetInt(LocomotionSettingsKey.EnableComfortMode.ToString(), manager.enableComfortMode ? 1 : 0) == 1;
            manager.snapTurnProvider.enableTurnAround = PlayerPrefs.GetInt(LocomotionSettingsKey.EnableTurnAround.ToString(), manager.snapTurnProvider.enableTurnAround ? 1 : 0) == 1;
        }

        public class LocomotionDefaults
        {
            public float MaxMoveSpeed = 5.0f;
            private const float MinMoveSpeed = 0.5f;
            private const float MaxTurnSpeed = 180f;
            private const float MaxSnapTurnAmount = 90f;
            private const float MaxGrabMoveRatio = 4f;
            private const float MinGrabMoveRatio = 0.5f;
        }

        public enum LocomotionSettingsKey
        {
            LeftHandLocomotionType,
            RightHandLocomotionType,
            LeftHandTurnStyle,
            RightHandTurnStyle,
            EnableStrafe,
            EnableComfortMode,
            EnableTurnAround
        }
    }
}