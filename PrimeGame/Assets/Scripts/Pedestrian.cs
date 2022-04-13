using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 0.1f)
        {
            rb.AddForce(transform.up * 3 * Time.deltaTime);
        }
        rb.AddForce(transform.forward * 3 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag.Equals("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            rb.constraints = RigidbodyConstraints.None;
            if (pc.IsSliding())
            {
                rb.AddForce(transform.up * 10);
            }
            else
            {
                rb.AddForce(transform.forward * 10);
            }
        }
        if(collision.transform.tag.Equals("Obstacle"))
        {
            transform.forward = -transform.forward;
            rb.AddForce(transform.forward);
        }
    }
}
