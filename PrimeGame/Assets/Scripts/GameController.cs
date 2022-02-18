using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static float GAME_SPEED = 1f;

    [SerializeField] private GameObject player;
    [SerializeField] private Text currencyCounterText;
    [SerializeField] private Text scoreCounterText;

    [HideInInspector] public int currencyCounter;
    [HideInInspector] public UnityEvent currencyEvent;
    [HideInInspector] public int scoreCounter;
    public GameObject Player { get => player; }

    public static GameController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        currencyEvent.AddListener(IncreaseCurrencyCounter);
        SetCurrencyCounter();
    }

    private void FixedUpdate()
    {
        UpdateScoreCounter();
    }

    private void IncreaseCurrencyCounter()
    {
        currencyCounter++;
        currencyCounterText.text = currencyCounter.ToString();
    }

    private void SetCurrencyCounter()
    {
        currencyCounter = PlayerPreferences.GetCurrencyAmount();
        currencyCounterText.text = currencyCounter.ToString();
    }

    private void UpdateScoreCounter()
    {
        scoreCounter = (int)(player.transform.position.z / 3);
        scoreCounterText.text = scoreCounter.ToString();
    }
}
