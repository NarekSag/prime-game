using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    private const float MOVEMENT_SPEED = 5f;
    private const float ROTATION_SPEED = 100f;

    [SerializeField] private GameObject obstacle;

    [SerializeField] private float movementSpeed;

    [SerializeField] private Vector3 endPosition;

    [SerializeField] private Vector3 endRotation;

    [SerializeField] private GameObject[] wheels;

    [SerializeField] private Vector3 wheelRotation;

    private BoxCollider boxCollider;

    public bool isTriggered;

    public Action triggeredEvent;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if(obstacle == null)
        {
            obstacle = transform.parent.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            endPosition = new Vector3(
                endPosition.x != 0 ? endPosition.x : obstacle.transform.localPosition.x,
                endPosition.y != 0 ? endPosition.y : obstacle.transform.localPosition.y,
                endPosition.z != 0 ? endPosition.z : obstacle.transform.localPosition.z);

            endRotation = new Vector3(
                endRotation.x != 0 ? endRotation.x : obstacle.transform.localRotation.x,
                endRotation.y != 0 ? endRotation.y : obstacle.transform.localRotation.y,
                endRotation.z != 0 ? endRotation.z : obstacle.transform.localRotation.z);

            boxCollider.enabled = false;
            isTriggered = true;
            triggeredEvent?.Invoke();
        }
    }

    private void Update()
    {
        if(isTriggered)
        {
            obstacle.transform.localPosition = Vector3.MoveTowards(obstacle.transform.localPosition, endPosition, (movementSpeed == 0 ? MOVEMENT_SPEED : movementSpeed) * Time.deltaTime);
            obstacle.transform.localRotation = Quaternion.RotateTowards(obstacle.transform.localRotation, Quaternion.Euler(endRotation), ROTATION_SPEED * Time.deltaTime);

            if(wheels != null)
            {
                foreach(GameObject wheel in wheels)
                {
                    if(obstacle.transform.localPosition != endPosition)
                        wheel.transform.Rotate(wheelRotation * (movementSpeed == 0 ? MOVEMENT_SPEED : movementSpeed) * Time.deltaTime);
                }
            }
            if (obstacle.transform.localPosition == endPosition && obstacle.transform.localRotation == Quaternion.Euler(endRotation))
            {
                isTriggered = false;
            }
        }
    }
}
