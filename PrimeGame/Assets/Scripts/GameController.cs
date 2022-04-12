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
    [SerializeField] private GameObject healthBar;
    [SerializeField] private TMP_TextEventHandler playTMP;
    [SerializeField] private TMP_TextEventHandler storeTMP;
    [SerializeField] private TMP_TextEventHandler storeBackTMP;
    [SerializeField] private TMP_TextEventHandler optionsTMP;
    [SerializeField] private Text currencyCounterText;
    [SerializeField] private Text scoreCounterText;
    [SerializeField] private Text healthCounterText;

    [HideInInspector] public int currencyCounter;
    [HideInInspector] public int scoreCounter;
    [HideInInspector] public bool doubleCurrency;
    [HideInInspector] public bool doubleScore;
    [HideInInspector] public bool gameStarted;
    [HideInInspector] public UnityEvent currencyEvent;
    [HideInInspector] public UnityEvent increaseHealthEvent;
    [HideInInspector] public UnityEvent decreaseHealthEvent;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public List<JSONReader.Item> storeItemList;

    private MainCamera mainCameraScript;
    private JSONReader jsonReader;

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
    }

    private void Start()
    {
        playerController = Player.GetComponent<PlayerController>();
        mainCameraScript = mainCamera.GetComponent<MainCamera>();
        jsonReader = GetComponent<JSONReader>();
        storeItemList = jsonReader.myItemList.item;

        currencyEvent.AddListener(IncreaseCurrencyCounter);
        increaseHealthEvent.AddListener(IncreaseHealth);
        decreaseHealthEvent.AddListener(DecreaseHealth);

        SetCurrencyCounter();
        SetHealthCounter();
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
        currencyCounter += doubleCurrency == false ? 1 : 2; //doubleCurrency is 1 || 2 if player got the doubleCurrency power up 
        currencyCounterText.text = currencyCounter.ToString();
    }

    private void IncreaseHealth()
    {
        playerController.health += 1;
        SetHealthCounter();
    }

    private void DecreaseHealth()
    {
        playerController.health -= 1;
        SetHealthCounter();
    }

    public void SetHealthCounter()
    {
        healthCounterText.text = $"x{playerController.health}";
    }

    private void SetCurrencyCounter()
    {
        currencyCounter = PlayerPreferences.GetCurrencyAmount();
        currencyCounterText.text = currencyCounter.ToString();
    }

    public void UpdateCurrency(int amount)
    {
        currencyCounter += amount;
        currencyCounterText.text = currencyCounter.ToString();
    }

    private void UpdateScoreCounter()
    {
        int score = (int)(player.transform.position.z / 3); // TO:DO IMPLEMENT DOUBLE SCORE LOGIC
        scoreCounter = score <= 0 ? 0 : score;
        scoreCounterText.text = scoreCounter.ToString();
    }

    public void OnPlayClicked()
    {
        gameStarted = true;
        mainCameraScript.SetTargetAngle(GAME);
        scoreCounterText.gameObject.SetActive(true);
        marketAnimator.Play("OpenDoorsAnim");
        playerController.SetGameState();
        healthBar.SetActive(true);
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

    private IEnumerator DoubleCurrency(float time)
    {
        doubleCurrency = true;
        yield return new WaitForSeconds(time);
        doubleCurrency = false;
    }

    public void SetDoubleCurrency()
    {
        StartCoroutine(DoubleCurrency(10));
    }

    private IEnumerator DoubleScore(float time)
    {
        doubleScore = true;
        yield return new WaitForSeconds(time);
        doubleScore = false;
    }

    public void SetDoubleScore()
    {
        StartCoroutine(DoubleScore(10));
    }

    public void OverrideItemJson()
    {
        jsonReader.Override(jsonReader.myItemList);
    }
}
