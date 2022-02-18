using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    private const string PLAYER = "Player";
    private const float ROTATE_SPEED = 1;

    [SerializeField] private GameObject mainObject;
    [SerializeField] private GameObject particle;

    private void Update()
    {
        float posY = Mathf.Lerp(.9f, 1.1f, Mathf.PingPong(Time.time, 1));
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        transform.Rotate(0, ROTATE_SPEED, 0, Space.World);
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
