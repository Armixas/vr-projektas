using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPeeking : MonoBehaviour
{
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] float fadeSpeed;
    [SerializeField] float sphereCheckSize;

    private Material _cameraFadeMat;
    private bool _isCameraFadedOut = false;
    private void Awake() => _cameraFadeMat = GetComponent<Renderer>().material;

    void Update()
    {
        if (Physics.CheckSphere(transform.position, sphereCheckSize, collisionLayer, QueryTriggerInteraction.Ignore))
        {
            CameraFade(1f);
            _isCameraFadedOut = true;
        }
        else
        {
            if (!_isCameraFadedOut)
                return;
            CameraFade(0f);
        }
    }

    public void CameraFade(float targetAlpha)
    {
        var fadeValue = Mathf.MoveTowards(_cameraFadeMat.GetFloat("_AlphaValue"), targetAlpha, Time.deltaTime * fadeSpeed);
        _cameraFadeMat.SetFloat("_AlphaValue", fadeValue);
        
        if(fadeValue <= 0.01f)
            _isCameraFadedOut = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.75f);
        Gizmos.DrawSphere(transform.position, sphereCheckSize);
    }
}
