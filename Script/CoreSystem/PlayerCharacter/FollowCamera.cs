using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    [Header("Player Target")]
    public Transform target;

    [Header("Smooth following Speed")]
    public float smoothSpeed = 0.125f;
    public float smoothSpeedWhileTeleport = 20f;
    public float lookAheadValue = 0.3f;
    public float lookUpValue = 1f;
    public float lookDownValue = -1f;
    [Space]

    [Header("Camera Offset")]
    public Vector3 offset;
    public Vector3 offsetUp;
    public Vector3 offsetDown;

    [Space]

    [Header("Shake Values")]
    public float shakeDuration = 0f;
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;
    [Space]

    private Transform camTransform;
    private PlayerLocomotion playerloco;
    private float lookAhead;
    private float lookAheadY = 0;
    private Vector3 originalPos;

    [HideInInspector]
    public Vector3 cameraFollowVelocity = Vector3.zero;

    private void Start()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerCharacter>().transform;
        }
        

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }

        lookAhead = lookAheadValue;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;       
        
        desiredPosition.x += lookAhead;
        desiredPosition.y += lookAheadY;
        
        transform.position = new Vector3(Vector3.SmoothDamp(transform.position, desiredPosition, ref cameraFollowVelocity, smoothSpeed).x, desiredPosition.y, desiredPosition.z);

        if (shakeDuration > 0)
        {
            camTransform.localPosition += Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
    }

    public void ChangeLookAheadDirection(bool facing)
    {
        if (facing)
            lookAhead = lookAheadValue;
        else
            lookAhead = lookAheadValue * -1f;
    }

    IEnumerator ChangeDirection(bool facing)
    {
        yield return new WaitForSeconds(0.2f);
        
        if (facing)
            lookAhead = lookAheadValue;
        else
            lookAhead = lookAheadValue * -1f;
    }

    public void ShakeCamera()
    {
        originalPos = camTransform.localPosition;
        shakeDuration = 0.1f;
    }

    public void AdjustYCamera(float ScrollCamY)
    {
        lookAheadY = Mathf.Lerp(lookAheadY, ScrollCamY, 0.2f);
    }
}
