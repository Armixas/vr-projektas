using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction
{
    public class Button : XRSimpleInteractable
    {
        class PressInfo
        {
            internal IXRHoverInteractor m_Interactor;
            internal bool m_InPressRegion = false;
        }

        [SerializeField]
        GameObject m_Button;

        [SerializeField]
        [Tooltip("Events to trigger when the button is pressed")]
        UnityEvent m_OnPress;

        [SerializeField]
        [Tooltip("Events to trigger when the button is released")]
        UnityEvent m_OnRelease;

        [SerializeField]
        [Tooltip("The color of the button when its pressed")]
        [ColorUsage(true, true)]
        Color m_PressedColor;

        [SerializeField]
        [Tooltip("The color of the button when its not pressed")]
        [ColorUsage(true, true)]
        Color m_UnpressedColor;

        [SerializeField]
        [Tooltip("Whether the button is pressed currently")]
        bool m_Toggled = false;

        [SerializeField]
        [Tooltip("Sounds to play when the button is activated or deactivated")]
        List<AudioClip> m_Sounds;

        public GameObject button
        {
            get => m_Button;
            set => m_Button = value;
        }

        public UnityEvent onPress => m_OnPress;
        public UnityEvent onRelease => m_OnRelease;

        public bool toggleValue
        {
            get => m_Toggled;
            set
            {
                m_Toggled = value;
                if (m_Toggled)
                    SetButtonColor(m_PressedColor);
                else
                    SetButtonColor(m_UnpressedColor);
            }
        }

        void SetButtonColor(Color color)
        {
            var renderer = button.GetComponent<Renderer>();
            renderer.material.SetColor("_EmissionColor", color);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_Toggled)
                SetButtonColor(m_PressedColor);
            else
                SetButtonColor(m_UnpressedColor);
        }

        public void Press()
        {
            m_Toggled = !m_Toggled;

            GetComponent<AudioSource>().PlayOneShot(m_Sounds[Random.Range(0, m_Sounds.Count - 1)], 0.4F);

            if (m_Toggled)
            {
                SetButtonColor(m_PressedColor);
                m_OnPress.Invoke();
            }
            else
            {
                SetButtonColor(m_UnpressedColor);
                m_OnRelease.Invoke();
            }
        }
    }
}