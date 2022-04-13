using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject[] pedestrians;

    private int secNum;
    private float zPos = 77f;
    private bool creatingSection;
    private bool destroyingSection;

    private List<GameObject> obstaclesList = new List<GameObject>();
    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameController.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if(obstaclesList.Count < 4)
        {
            GenerateSection();
        }
        if(obstaclesList != null && obstaclesList.Count >= 2 && playerObject.transform.position.z > obstaclesList[1].transform.position.z)
        {
            DestroyObstacle();
        }
    }

    private void GenerateSection()
    {
        secNum = Random.Range(0, obstacles.Length);
        var obstacleInstance = Instantiate(obstacles[secNum], new Vector3(0, 0, zPos), Quaternion.identity);
        float oldZPos = zPos;
        if (obstacles[secNum].name.Contains("DoubleObstacle"))
        {
            zPos += 154;
        }
        else
        {
            zPos += 77;
        }
        if(!(obstacles[secNum].name.Contains("DoubleObstacle2") || obstacles[secNum].name.Contains("DoubleObstacle3")))
            GeneratePedestrians(10, oldZPos ,zPos, obstacleInstance.transform);
        obstaclesList.Add(obstacleInstance);
    }

    private void GeneratePedestrians(int amount,float minPos, float maxPos, Transform parent)
    {
        for(int i = 0; i < amount; i++)
        {
            int randPed = Random.Range(0, pedestrians.Length);
            float randZPos = Random.Range(minPos, maxPos);
            float randYRot = Random.Range(0, 180);
            var pedestrianInstance = Instantiate(pedestrians[randPed], new Vector3(0, 0, randZPos), Quaternion.identity, parent);
            pedestrianInstance.transform.eulerAngles = new Vector3(0, randYRot, 0);
        }
    }

    private void DestroyObstacle()
    {
        var firstObstacle = obstaclesList[0];
        obstaclesList.RemoveAt(0);
        Destroy(firstObstacle);
    }
}
