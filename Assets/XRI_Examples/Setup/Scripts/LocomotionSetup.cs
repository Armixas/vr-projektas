using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace UnityEngine.XR.Content.Interaction
{
    /// <summary>
    /// Use this class to present locomotion control schemes and configuration preferences,
    /// and respond to player input in the UI to set them.
    /// </summary>
    /// <seealso cref="LocomotionManager"/>
    public class LocomotionSetup : MonoBehaviour
    {
        const float k_MaxMoveSpeed = 5.0f;
        const float k_MinMoveSpeed = 0.5f;
        const float k_MaxTurnSpeed = 180f;
        const float k_MaxSnapTurnAmount = 90f;
        const float k_MaxGrabMoveRatio = 4f;
        const float k_MinGrabMoveRatio = 0.5f;

        const string k_SpeedFormat = "###.0";
        const string k_DegreeFormat = "###";
        const string k_GrabMoveRatioFormat = "###.0";
        const string k_MoveSpeedUnitLabel = " m/s";
        const string k_TurnSpeedUnitLabel = "°/s";
        const string k_SnapTurnAmountLabel = "°";
        const string k_GrabMoveRatioLabel = " : 1.0";

        const string k_GravityLabel = "Rig Gravity";

        [SerializeField]
        [Tooltip("Stores the behavior that will be used to configure locomotion control schemes and configuration preferences.")]
        LocomotionManager m_Manager;

        [SerializeField]
        [Tooltip("Stores the GameObject reference used to turn on and off the movement direction toggle in the 3D UI for the left hand.")]
        GameObject m_LeftHandMovementDirectionSelection;

        [SerializeField]
        [Tooltip("Stores the GameObject reference used to turn on and off the movement direction toggle in the 3D UI for the right hand.")]
        GameObject m_RightHandMovementDirectionSelection;

        [SerializeField]
        [Tooltip("Stores the GameObject reference used to turn on and off the turn style toggle in the 3D UI for the left hand.")]
        GameObject m_LeftHandTurnStyleSelection;

        [SerializeField]
        [Tooltip("Stores the GameObject reference used to turn on and off the turn style toggle in the 3D UI for the right hand.")]
        GameObject m_RightHandTurnStyleSelection;

        [SerializeField]
        [Tooltip("Stores the toggle lever used to choose the locomotion type between move/strafe and teleport/turn for the left hand.")]
        XRLever m_LeftHandLocomotionTypeToggle;

        [SerializeField]
        [Tooltip("Stores the toggle lever used to choose the locomotion type between move/strafe and teleport/turn for the right hand.")]
        XRLever m_RightHandLocomotionTypeToggle;

        [SerializeField]
        [Tooltip("Stores the toggle lever used to choose the movement direction between head-relative and hand-relative for the left hand.")]
        XRLever m_LeftHandMovementDirectionToggle;

        [SerializeField]
        [Tooltip("Stores the toggle lever used to choose the movement direction between head-relative and hand-relative for the right hand.")]
        XRLever m_RightHandMovementDirectionToggle;

        [SerializeField]
        [Tooltip("Stores the toggle lever used to choose the turn style between continuous and snap for the left hand.")]
        XRLever m_LeftHandTurnStyleToggle;

        [SerializeField]
        [Tooltip("Stores the toggle lever used to choose the turn style between continuous and snap for the right hand.")]
        XRLever m_RightHandTurnStyleToggle;

        [SerializeField]
        [Tooltip("Stores the button toggle used to enable strafing movement.")]
        Button m_StrafeToggle;

        [SerializeField]
        [Tooltip("Stores the button toggle used to enable comfort mode.")]
        Button m_ComfortModeToggle;

        [SerializeField]
        [Tooltip("Stores the button toggle used to enable instant turn-around.")]
        Button m_TurnAroundToggle;

        [SerializeField]
        [Tooltip("The label that shows the current turn around toggle value.")]
        TextMeshPro m_StrafeLabel;

        [SerializeField]
        [Tooltip("The label that shows the current turn around toggle value.")]
        TextMeshPro m_ComfortModeLabel;

        [SerializeField]
        [Tooltip("The label that shows the current turn around toggle value.")]
        TextMeshPro m_TurnAroundLabel;


        [SerializeField] [Tooltip("The label that shows the current turn around toggle value.")]
        private GameObject _locomotionPreferencesManager;

        /*void Awake() => _locomotionPreferencesManager.GetComponent<UserPreferencesLoader>();*/

        void ConnectControlEvents()
        {
            m_LeftHandLocomotionTypeToggle.onLeverActivate.AddListener(EnableLeftHandMoveAndStrafe);
            m_LeftHandLocomotionTypeToggle.onLeverDeactivate.AddListener(EnableLeftHandTeleportAndTurn);
            m_RightHandLocomotionTypeToggle.onLeverActivate.AddListener(EnableRightHandMoveAndStrafe);
            m_RightHandLocomotionTypeToggle.onLeverDeactivate.AddListener(EnableRightHandTeleportAndTurn);

            m_LeftHandMovementDirectionToggle.onLeverActivate.AddListener(SetLeftMovementDirectionHeadRelative);
            m_LeftHandMovementDirectionToggle.onLeverDeactivate.AddListener(SetLeftMovementDirectionHandRelative);
            m_RightHandMovementDirectionToggle.onLeverActivate.AddListener(SetRightMovementDirectionHeadRelative);
            m_RightHandMovementDirectionToggle.onLeverDeactivate.AddListener(SetRightMovementDirectionHandRelative);

            m_LeftHandTurnStyleToggle.onLeverActivate.AddListener(EnableLeftHandContinuousTurn);
            m_LeftHandTurnStyleToggle.onLeverDeactivate.AddListener(EnableLeftHandSnapTurn);
            m_RightHandTurnStyleToggle.onLeverActivate.AddListener(EnableRightHandContinuousTurn);
            m_RightHandTurnStyleToggle.onLeverDeactivate.AddListener(EnableRightHandSnapTurn);

            m_StrafeToggle.onPress.AddListener(EnableStrafe);
            m_StrafeToggle.onRelease.AddListener(DisableStrafe);
            m_ComfortModeToggle.onPress.AddListener(EnableComfort);
            m_ComfortModeToggle.onRelease.AddListener(DisableComfort);
            m_TurnAroundToggle.onPress.AddListener(EnableTurnAround);
            m_TurnAroundToggle.onRelease.AddListener(DisableTurnAround);
        }

        void DisconnectControlEvents()
        {
            m_LeftHandLocomotionTypeToggle.onLeverActivate.RemoveListener(EnableLeftHandMoveAndStrafe);
            m_LeftHandLocomotionTypeToggle.onLeverDeactivate.RemoveListener(EnableLeftHandTeleportAndTurn);
            m_RightHandLocomotionTypeToggle.onLeverActivate.RemoveListener(EnableRightHandMoveAndStrafe);
            m_RightHandLocomotionTypeToggle.onLeverDeactivate.RemoveListener(EnableRightHandTeleportAndTurn);

            m_LeftHandMovementDirectionToggle.onLeverActivate.RemoveListener(SetLeftMovementDirectionHeadRelative);
            m_LeftHandMovementDirectionToggle.onLeverDeactivate.RemoveListener(SetLeftMovementDirectionHandRelative);
            m_RightHandMovementDirectionToggle.onLeverActivate.RemoveListener(SetRightMovementDirectionHeadRelative);
            m_RightHandMovementDirectionToggle.onLeverDeactivate.RemoveListener(SetRightMovementDirectionHandRelative);

            m_LeftHandTurnStyleToggle.onLeverActivate.RemoveListener(EnableLeftHandContinuousTurn);
            m_LeftHandTurnStyleToggle.onLeverDeactivate.RemoveListener(EnableLeftHandSnapTurn);
            m_RightHandTurnStyleToggle.onLeverActivate.RemoveListener(EnableRightHandContinuousTurn);
            m_RightHandTurnStyleToggle.onLeverDeactivate.RemoveListener(EnableRightHandSnapTurn);

            m_StrafeToggle.onPress.RemoveListener(EnableStrafe);
            m_StrafeToggle.onRelease.RemoveListener(DisableStrafe);
            m_ComfortModeToggle.onPress.RemoveListener(EnableComfort);
            m_ComfortModeToggle.onRelease.RemoveListener(DisableComfort);
            m_TurnAroundToggle.onPress.RemoveListener(EnableTurnAround);
            m_TurnAroundToggle.onRelease.RemoveListener(DisableTurnAround);
        }

        void InitializeControls()
        {
            var isLeftHandMoveAndStrafe = m_Manager.leftHandLocomotionType == LocomotionManager.LocomotionType.MoveAndStrafe;
            var isRightHandMoveAndStrafe = m_Manager.rightHandLocomotionType == LocomotionManager.LocomotionType.MoveAndStrafe;
            m_LeftHandLocomotionTypeToggle.value = isLeftHandMoveAndStrafe;
            m_RightHandLocomotionTypeToggle.value = isRightHandMoveAndStrafe;

            m_LeftHandTurnStyleSelection.SetActive(!isLeftHandMoveAndStrafe);
            m_RightHandTurnStyleSelection.SetActive(!isRightHandMoveAndStrafe);

            m_LeftHandTurnStyleToggle.value = (m_Manager.leftHandTurnStyle == LocomotionManager.TurnStyle.Smooth);
            m_RightHandTurnStyleToggle.value = (m_Manager.rightHandTurnStyle == LocomotionManager.TurnStyle.Smooth);
            m_StrafeToggle.toggleValue = m_Manager.dynamicMoveProvider.enableStrafe;
            m_StrafeLabel.text = $"Strafe\n{(m_Manager.dynamicMoveProvider.enableStrafe ? "Enabled" : "Disabled")}";
            m_ComfortModeToggle.toggleValue = (m_Manager.enableComfortMode);
            m_ComfortModeLabel.text = $"Comfort Mode\n{(m_Manager.enableComfortMode ? "Enabled" : "Disabled")}";
            m_TurnAroundToggle.toggleValue = m_Manager.snapTurnProvider.enableTurnAround;
            m_TurnAroundLabel.text = $"Turn Around \n{(m_Manager.snapTurnProvider.enableTurnAround ? "Enabled" : "Disabled")}";
        }

        protected void OnEnable()
        {
            if (!ValidateManager())
                return;

            ConnectControlEvents();
            InitializeControls();
        }

        protected void OnDisable()
        {
            DisconnectControlEvents();
        }

        bool ValidateManager()
        {
            if (m_Manager == null)
            {
                Debug.LogError($"Reference to the {nameof(LocomotionManager)} is not set or the object has been destroyed," +
                    " configuring locomotion settings from the menu will not be possible." +
                    " Ensure the value has been set in the Inspector.", this);
                return false;
            }

            if (m_Manager.dynamicMoveProvider == null)
            {
                Debug.LogError($"Reference to the {nameof(LocomotionManager.dynamicMoveProvider)} is not set or the object has been destroyed," +
                    " configuring locomotion settings from the menu will not be possible." +
                    $" Ensure the value has been set in the Inspector on {m_Manager}.", this);
                return false;
            }

            if (m_Manager.smoothTurnProvider == null)
            {
                Debug.LogError($"Reference to the {nameof(LocomotionManager.smoothTurnProvider)} is not set or the object has been destroyed," +
                    " configuring locomotion settings from the menu will not be possible." +
                    $" Ensure the value has been set in the Inspector on {m_Manager}.", this);
                return false;
            }

            if (m_Manager.snapTurnProvider == null)
            {
                Debug.LogError($"Reference to the {nameof(LocomotionManager.snapTurnProvider)} is not set or the object has been destroyed," +
                    " configuring locomotion settings from the menu will not be possible." +
                    $" Ensure the value has been set in the Inspector on {m_Manager}.", this);
                return false;
            }

            if (m_Manager.twoHandedGrabMoveProvider == null)
            {
                Debug.LogError($"Reference to the {nameof(LocomotionManager.twoHandedGrabMoveProvider)} is not set or the object has been destroyed," +
                               " configuring locomotion settings from the menu will not be possible." +
                               $" Ensure the value has been set in the Inspector on {m_Manager}.", this);
                return false;
            }

            return true;
        }

        void EnableLeftHandMoveAndStrafe()
        {
            m_Manager.leftHandLocomotionType = LocomotionManager.LocomotionType.MoveAndStrafe;
            m_LeftHandMovementDirectionSelection.SetActive(true);
            m_LeftHandTurnStyleSelection.SetActive(false);
        }

        void EnableRightHandMoveAndStrafe()
        {
            m_Manager.rightHandLocomotionType = LocomotionManager.LocomotionType.MoveAndStrafe;
            m_RightHandMovementDirectionSelection.SetActive(true);
            m_RightHandTurnStyleSelection.SetActive(false);
        }

        void EnableLeftHandTeleportAndTurn()
        {
            m_Manager.leftHandLocomotionType = LocomotionManager.LocomotionType.TeleportAndTurn;
            m_LeftHandMovementDirectionSelection.SetActive(false);
            m_LeftHandTurnStyleSelection.SetActive(true);
        }

        void EnableRightHandTeleportAndTurn()
        {
            m_Manager.rightHandLocomotionType = LocomotionManager.LocomotionType.TeleportAndTurn;
            m_RightHandMovementDirectionSelection.SetActive(false);
            m_RightHandTurnStyleSelection.SetActive(true);
        }

        void EnableLeftHandContinuousTurn()
        {
            m_Manager.leftHandTurnStyle = LocomotionManager.TurnStyle.Smooth;
        }

        void EnableRightHandContinuousTurn()
        {
            m_Manager.rightHandTurnStyle = LocomotionManager.TurnStyle.Smooth;
        }

        void EnableLeftHandSnapTurn()
        {
            m_Manager.leftHandTurnStyle = LocomotionManager.TurnStyle.Snap;
        }

        void EnableRightHandSnapTurn()
        {
            m_Manager.rightHandTurnStyle = LocomotionManager.TurnStyle.Snap;
        }

        void SetLeftMovementDirectionHeadRelative()
        {
            m_Manager.dynamicMoveProvider.leftHandMovementDirection = DynamicMoveProvider.MovementDirection.HeadRelative;
        }

        void SetLeftMovementDirectionHandRelative()
        {
            m_Manager.dynamicMoveProvider.leftHandMovementDirection = DynamicMoveProvider.MovementDirection.HandRelative;
        }

        void SetRightMovementDirectionHeadRelative()
        {
            m_Manager.dynamicMoveProvider.rightHandMovementDirection = DynamicMoveProvider.MovementDirection.HeadRelative;
        }

        void SetRightMovementDirectionHandRelative()
        {
            m_Manager.dynamicMoveProvider.rightHandMovementDirection = DynamicMoveProvider.MovementDirection.HandRelative;
        }

        void EnableStrafe()
        {
            m_Manager.dynamicMoveProvider.enableStrafe = true;
            m_StrafeLabel.text = $"Strafe\n{(m_Manager.dynamicMoveProvider.enableStrafe ? "Enabled" : "Disabled")}";
        }

        void DisableStrafe()
        {
            m_Manager.dynamicMoveProvider.enableStrafe = false;
            m_StrafeLabel.text = $"Strafe\n{(m_Manager.dynamicMoveProvider.enableStrafe ? "Enabled" : "Disabled")}";
        }

        void EnableComfort()
        {
            m_Manager.enableComfortMode = true;
            m_ComfortModeLabel.text = $"Comfort Mode\n{(m_Manager.enableComfortMode ? "Enabled" : "Disabled")}";
        }

        void DisableComfort()
        {
            m_Manager.enableComfortMode = false;
            m_ComfortModeLabel.text = $"Comfort Mode\n{(m_Manager.enableComfortMode ? "Enabled" : "Disabled")}";
        }

        void EnableTurnAround()
        {
            m_Manager.snapTurnProvider.enableTurnAround = true;
            m_TurnAroundLabel.text = $"Turn Around \n{(m_Manager.snapTurnProvider.enableTurnAround ? "Enabled" : "Disabled")}";
        }

        void DisableTurnAround()
        {
            m_Manager.snapTurnProvider.enableTurnAround = false;
            m_TurnAroundLabel.text = $"Turn Around \n{(m_Manager.snapTurnProvider.enableTurnAround ? "Enabled" : "Disabled")}";
        }        
    }
}
