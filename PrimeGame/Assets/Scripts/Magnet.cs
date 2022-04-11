using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisableAfter(10));    
    }

    private IEnumerator DisableAfter(float time)
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Collectible"))
        {
            other.GetComponent<CurrencyController>().attracted = true;
        }
    }
}
