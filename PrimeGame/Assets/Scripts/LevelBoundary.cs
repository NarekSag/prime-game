using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundary : MonoBehaviour
{
    public static float LEFT_SIDE = -4.5f;
    public static float RIGHT_SIDE = 4.5f;
    private float internalLeft;
    private float internalRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        internalLeft = LEFT_SIDE;
        internalRight = RIGHT_SIDE;
    }
}
