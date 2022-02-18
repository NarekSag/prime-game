using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TruckObstacle : MonoBehaviour
{
    private const float LIFTGATE_SPEED = 5;

    [SerializeField] GameObject liftGate;
    [SerializeField] GameObject[] fallingObstacles;

    private List<GameObject> instantiatedObstacles = new List<GameObject>();
    private Vector3 randPos = Vector3.zero;

    private ObstacleTrigger obstacle;
    private bool isInvoking;

    private void Start()
    {
        obstacle = GetComponent<ObstacleTrigger>();
        obstacle.triggeredEvent += InvokeFallingObstacle;
    }

    private void Update()
    {
        if(obstacle.isTriggered)
        {
            liftGate.transform.localScale = Vector3.Lerp(liftGate.transform.localScale, new Vector3(liftGate.transform.localScale.x, liftGate.transform.localScale.y, 0), LIFTGATE_SPEED * Time.deltaTime);

            if (instantiatedObstacles.Count != 0)
            {
                instantiatedObstacles.Last().transform.position = Vector3.Lerp(instantiatedObstacles.Last().transform.position, randPos, LIFTGATE_SPEED * Time.deltaTime);
            }
        }
        else
        {
            CancelInvoke();
        }    
    }

    private void InvokeFallingObstacle()
    {
        InvokeRepeating("InstantiateFallingObstacle", 2, 1f);
        obstacle.triggeredEvent -= InvokeFallingObstacle;
    }

    private void InstantiateFallingObstacle()
    {
        int randObject = Random.Range(0, fallingObstacles.Length);
        GameObject fallingObstacle = Instantiate(fallingObstacles[randObject], this.transform.parent);
        fallingObstacle.transform.position = new Vector3(transform.position.x, 2, transform.position.z - 5);
        instantiatedObstacles.Add(fallingObstacle);
        randPos = new Vector3(Random.Range(LevelBoundary.LEFT_SIDE, LevelBoundary.RIGHT_SIDE), transform.position.y, transform.position.z - 10);
    }
}
