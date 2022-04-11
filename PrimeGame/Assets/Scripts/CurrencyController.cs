using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    private const string PLAYER = "Player";
    private const float ROTATE_SPEED = 1;

    [SerializeField] private GameObject mainObject;
    [SerializeField] private GameObject particle;

    private float startingYPos;

    public bool attracted;

    private void Start()
    {
        startingYPos = transform.position.y;
    }

    private void Update()
    {
        if(!attracted)
        {
            float posY = Mathf.Lerp(startingYPos, startingYPos + .2f, Mathf.PingPong(Time.time, 1));
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            transform.Rotate(0, ROTATE_SPEED, 0, Space.World);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, GameController.instance.Player.transform.position, 10 * Time.deltaTime);
            if(Vector3.Distance(transform.position, GameController.instance.Player.transform.position) < 1.5f)
            {
                GameController.instance.currencyEvent.Invoke();
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(PLAYER))
        {
            GameController.instance.currencyEvent.Invoke();
            Destroy(this.gameObject);
        }
    }
}
