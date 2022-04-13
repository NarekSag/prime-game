using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private const float STORE_ANGLE = 270f;
    private const float GAME_ANGLE = 359.99f;
    private const float OPTIONS_ANGLE = 90;
    private const float DEFAULT_ANGLE = 180;

    private Vector3 offset;// = new Vector3(0, 4, -3.5f);

    private Vector3 currentAngle;
    private Vector3 startingAngle = new Vector3(25, DEFAULT_ANGLE, 0);
    private Vector3 targetAngle = Vector3.zero;

    private bool targetEventInvoked;
    private bool inTween;

    public Action targetDestinationEvent;

    public void Start()
    {
        currentAngle = transform.eulerAngles = startingAngle;
        SetCameraOffset(-3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetAngle != Vector3.zero && transform.eulerAngles != targetAngle)
        {
            if(GameController.instance.gameStarted)
            {
                if (inTween)
                {
                    SetCamera();
                }
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetAngle), 5 * Time.deltaTime);
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetAngle), 100 * Time.deltaTime);
            }
        }
        if((Vector3.Distance(transform.eulerAngles, targetAngle) < 0.1f) && !targetEventInvoked)
        {
            targetDestinationEvent?.Invoke();
            targetEventInvoked = true;
        }
    }

    private void LateUpdate()
    {
        if (GameController.instance.gameStarted && transform.eulerAngles == targetAngle)
        {
            inTween = false;
            transform.position = GameController.instance.Player.transform.position + offset;
        }

    }

    public void SetTargetAngle(string target = "Default")
    {
        targetEventInvoked = false;
        inTween = true;
        switch (target)
        {
            case "Game":
                targetAngle = new Vector3(currentAngle.x, GAME_ANGLE, currentAngle.z);
                break;
            case "Store":
                targetAngle = new Vector3(currentAngle.x, STORE_ANGLE, currentAngle.z);
                break;
            case "Options":
                targetAngle = new Vector3(currentAngle.x, OPTIONS_ANGLE, currentAngle.z);
                break;
            default:
                targetAngle = new Vector3(currentAngle.x, DEFAULT_ANGLE, currentAngle.z);
                break;
        }
    }

    public void SetCamera()
    {
        transform.position = Vector3.Lerp(transform.position, GameController.instance.Player.transform.position + offset, 10 * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetAngle), 10 * Time.deltaTime);
    }

    public void SetCameraOffset(float offsetZ)
    {
        offset = new Vector3(0, 4, offsetZ);
    }
    
    public void SetCameraForwardView(bool forward = true)
    {
        if (forward)
        {
            SetTargetAngle("Game");
            SetCameraOffset(-3.5f);
        }
        else
        {
            SetTargetAngle();
            SetCameraOffset(10);
        }
    }
}
