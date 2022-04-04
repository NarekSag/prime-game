using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public const string GAME = "Game";
    public const string STORE = "Store";
    public const string DEFAULT = "Default";

    public static float GAME_SPEED = 1f;
    [Header("Main Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator marketAnimator;
    [SerializeField] private Animator storeAnimator;

    [Space, Header("UI")]
    [SerializeField] private GameObject storeUI;
    [SerializeField] private TMP_TextEventHandler playTMP;
    [SerializeField] private TMP_TextEventHandler storeTMP;
    [SerializeField] private TMP_TextEventHandler storeBackTMP;
    [SerializeField] private TMP_TextEventHandler optionsTMP;
    [SerializeField] private Text currencyCounterText;
    [SerializeField] private Text scoreCounterText;

    [HideInInspector] public int currencyCounter;
    [HideInInspector] public UnityEvent currencyEvent;
    [HideInInspector] public int scoreCounter;
    [HideInInspector] public bool gameStarted;

    private MainCamera mainCameraScript;

    public GameObject Player { get => player; }
    public Camera MainCamera { get => mainCamera; }

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
        mainCameraScript = mainCamera.GetComponent<MainCamera>();
        scoreCounterText.gameObject.SetActive(false);
        storeUI.SetActive(false);
    }

    private void FixedUpdate()
    {
        UpdateScoreCounter();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
        int score = (int)(player.transform.position.z / 3);
        scoreCounter = score <= 0 ? 0 : score;
        scoreCounterText.text = scoreCounter.ToString();
    }

    public void OnPlayClicked()
    {
        gameStarted = true;
        mainCameraScript.SetTargetAngle(GAME);
        scoreCounterText.gameObject.SetActive(true);
        marketAnimator.Play("OpenDoorsAnim");
        player.GetComponent<PlayerController>().SetGameState();
        DisableAllTMP();
    }

    public void OnStoreClicked()
    {
        mainCameraScript.SetTargetAngle(STORE);
        storeAnimator.Play("StoreDoorAnim");
        player.GetComponent<PlayerController>().SetStoreEnterState();
        storeTMP.enabled = false;
        storeBackTMP.enabled = false;
        mainCameraScript.targetDestinationEvent = null;
        mainCameraScript.targetDestinationEvent += TargetOnStore;
    }

    public void OnStoreBackClicked()
    {
        mainCameraScript.SetTargetAngle(DEFAULT);
        storeUI.SetActive(false);
        player.GetComponent<PlayerController>().SetStoreExitState();
        mainCameraScript.targetDestinationEvent = null;
        mainCameraScript.targetDestinationEvent += EnableStoreTMP;
    }

    public void EnableStoreTMP()
    {
        storeTMP.enabled = true;
    }

    public void TargetOnStore()
    {
        storeBackTMP.enabled = true;
        storeUI.SetActive(true);
    }

    public void DisableAllTMP()
    {
        playTMP.enabled = false;
        storeTMP.enabled = false;
        storeBackTMP.enabled = false;
        optionsTMP.enabled = false;
    }

    public void GameOver()
    {

    }
}
