using System;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace Settings
{
    public class LocomotionPreferencesManager : MonoBehaviour
    {
        public float MaxMoveSpeed = 5.0f;

        private ActionBasedSnapTurnProvider _snapTurnProvider;
        private ActionBasedContinuousTurnProvider _continuousTurnProvider;
        private DynamicMoveProvider _moveProvider;
        // Start is called before the first frame update
        
        [SerializeField]
        private float MoveSpeed = 5.0f;
        [SerializeField]
        private float TurnSpeed = 180f;
        [SerializeField]
        private float SnapTurnAmount = 90f;
        [SerializeField]
        private GameObject _XROrigin;


        void Awake() => InitializeComponents();
        void Start()
        {
            LoadDefaultSettings();
            if (!HasAllPrefs()) return;
        }

        void InitializeComponents()
        {
            if (_XROrigin != null)
            {
                _snapTurnProvider = _XROrigin.GetComponent<ActionBasedSnapTurnProvider>();
                _moveProvider = _XROrigin.GetComponent<DynamicMoveProvider>();
                _continuousTurnProvider = _XROrigin.GetComponent<ActionBasedContinuousTurnProvider>();
            }
            else
            {
                Debug.LogError("XROrigin is null. Assign the GameObject to script.");
            }
        }

        void LoadDefaultSettings()
        {
            _continuousTurnProvider.turnSpeed = TurnSpeed;
            _moveProvider.moveSpeed = MoveSpeed;
            _snapTurnProvider.turnAmount = SnapTurnAmount;
        }

        bool HasAllPrefs()
        {
            string[] enumNames = Enum.GetNames(typeof(LocomotionSettingsKey));
            int count = 0;
            foreach (string key in enumNames)
            {
                if (PlayerPrefs.HasKey(key))
                    count++;
            }

            return count == enumNames.Length;
        }

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