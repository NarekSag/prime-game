using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI toastMessage;

    private Animator animator;

    private void Start()
    {
        animator = toastMessage.GetComponent<Animator>();
        Color color = toastMessage.color;
        color.a = 0;
        toastMessage.color = color;
    }

    public void ShowToastMessage(string text)
    {
        toastMessage.gameObject.SetActive(true);
        toastMessage.text = text;
        animator.Play("ToastMessage");
    }
}
