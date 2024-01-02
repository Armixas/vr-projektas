using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

        [SerializeField] private float MoveSpeed = 5.0f;
        [SerializeField] private float TurnSpeed = 90f;
        [SerializeField] private float SnapTurnAmount = 45f;
        [SerializeField] private GameObject _XROrigin;

        private string filePath;


        void Awake()
        {
            filePath = Application.persistentDataPath + "/locomotionSettings.dat";
            InitializeComponents();
        }

    void Start()
        {
            var locomanager = _XROrigin.GetComponent<LocomotionManager>();
            
            
            LoadDefaultSettings();
                
            if (!HasAllPrefs()) return;
            
            LoadPreferences(locomanager);
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
            SaveSettingBinary(manager);
        }

        public void SaveSettingBinary(LocomotionManager manager)
        {
            // Alternative way to save data as PlayerPref didn't fully work
            LocomotionSettingsData settingsData = new LocomotionSettingsData();
            settingsData.leftHandLocomotionType = (int)manager.leftHandLocomotionType;
            settingsData.rightHandLocomotionType = (int)manager.rightHandLocomotionType;
            settingsData.leftHandTurnStyle = (int)manager.leftHandTurnStyle;
            settingsData.rightHandTurnStyle = (int)manager.rightHandTurnStyle;
            settingsData.enableStrafe = manager.dynamicMoveProvider.enableStrafe ? 1 : 0;
            settingsData.enableComfortMode = manager.enableComfortMode ? 1 : 0;
            settingsData.enableTurnAround = manager.snapTurnProvider.enableTurnAround ? 1 : 0;
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            formatter.Serialize(fileStream, settingsData);
            fileStream.Close();
        }
        public void LoadPreferences(LocomotionManager manager)
        {
        
            if (!File.Exists(filePath)) return;
            
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            LocomotionSettingsData settingsData = (LocomotionSettingsData)formatter.Deserialize(fileStream);
            fileStream.Close();

            manager.leftHandLocomotionType = (LocomotionManager.LocomotionType)PlayerPrefs.GetInt(LocomotionSettingsKey.LeftHandLocomotionType.ToString(), (int)manager.leftHandLocomotionType);
            manager.rightHandLocomotionType = (LocomotionManager.LocomotionType)PlayerPrefs.GetInt(LocomotionSettingsKey.RightHandLocomotionType.ToString(), (int)manager.rightHandLocomotionType);

            // Turn style preferences
            manager.leftHandTurnStyle = (LocomotionManager.TurnStyle) settingsData.leftHandTurnStyle;
            manager.rightHandTurnStyle = (LocomotionManager.TurnStyle) settingsData.rightHandTurnStyle;

            manager.dynamicMoveProvider.enableStrafe = settingsData.enableStrafe == 1;
            manager.enableComfortMode = settingsData.enableComfortMode == 1;
            manager.snapTurnProvider.enableTurnAround = settingsData.enableTurnAround == 1;

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
        [System.Serializable]
        public class LocomotionSettingsData
        {
            public int leftHandLocomotionType;
            public int rightHandLocomotionType;
            public int leftHandTurnStyle;
            public int rightHandTurnStyle;
            public int enableStrafe;
            public int enableComfortMode;
            public int enableTurnAround;
        }
    }
}