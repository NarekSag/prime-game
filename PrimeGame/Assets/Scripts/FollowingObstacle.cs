using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObstacle : MonoBehaviour
{
    private const float MOVEMENT_SPEED = 9f;

    [SerializeField] private GameObject followingObject;
    [SerializeField] private ObstacleTrigger obstacle;
    [SerializeField] private GameObject endPoint;

    private CapsuleCollider capsuleCollider;
    private bool isStarted;
    private Vector3 endPosition;
    private Vector3 obstacleRotation;

    private Transform cameraTransform;
    private Vector3 startCameraPosition;
    private Vector3 endCameraPosition;
    private Vector3 startCameraRotation;
    private Vector3 endCameraRotation;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        followingObject.gameObject.SetActive(false);

        cameraTransform = GameController.instance.MainCamera.transform;
        startCameraPosition = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z + 10);
        endCameraPosition = cameraTransform.position;
        startCameraRotation = new Vector3(cameraTransform.rotation.eulerAngles.x, 180, cameraTransform.rotation.eulerAngles.z);
        endCameraRotation = cameraTransform.rotation.eulerAngles;

        obstacle.triggeredEvent += InitFollowingObstacle;
    }

    private void InitFollowingObstacle()
    {
        StartCoroutine(InitObstacle(1));
        obstacle.triggeredEvent -= InitFollowingObstacle;
    }

    private void Update()
    {
        if (isStarted)
        {
            if(GameController.instance.Player.transform.position.z < endPoint.transform.position.z)
            {
                float y = Mathf.PingPong(Time.time * (MOVEMENT_SPEED * 0.5f), 4 - 2) + 2;
                Vector3 yPos = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, yPos, 100 * Time.deltaTime);

                endPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 145);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPosition, MOVEMENT_SPEED * Time.deltaTime);

                transform.Rotate(0, -100 * Time.deltaTime, 0);
            }
            else
            {
                GameController.instance.MainCamera.GetComponent<MainCamera>().SetCameraForwardView();
            }

            if(transform.position.z >= endPoint.transform.position.z)
            {
                Destroy(this.gameObject);
            }
            if(GameController.instance.Player.transform.position.z <= transform.position.z)
            {
                GameOver();
            }
        }

    }

    private IEnumerator InitObstacle(float time)
    {
        yield return new WaitForSeconds(time);
        capsuleCollider.enabled = true;
        followingObject.gameObject.SetActive(true);
        isStarted = true;
        GameController.instance.MainCamera.GetComponent<MainCamera>().SetCameraForwardView(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag.Equals("Player"))
            GameOver();
    }

    private void GameOver()
    {
        GameController.instance.gameStarted = false;
        GameController.instance.Player.GetComponent<PlayerController>().PlayGameOverAnim();
    }
}
