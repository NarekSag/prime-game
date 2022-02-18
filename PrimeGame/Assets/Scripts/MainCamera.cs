using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Vector3 offset = new Vector3(0, 4, -7);

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + offset;
    }
}
