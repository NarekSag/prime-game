using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Const
    private const string RUN = "Run";
    private const string JUMP = "Jump";
    private const string SLIDE = "Slide";
    private const string FALL_BACK = "Stumble";//"Fall Back";
    private const string FALL_FORWARD = "Fall Forward";
    private const string OBSTACLE = "Obstacle";
    private const string CATWALK_FORWARD = "CatWalkForward";
    private const string CATWALK_BACKWARD = "CatWalkBackward";
    private const float FORCE = 100;
    private const float ROTATION_SPEED = 10;
    #endregion

    private Quaternion leftRot = Quaternion.AngleAxis(-45, Vector3.up);
    private Quaternion rightRot = Quaternion.AngleAxis(45, Vector3.up);
    private Quaternion forwardRot = Quaternion.AngleAxis(0, Vector3.up);

    [SerializeField] private GameObject tShirt;
    [SerializeField] private Material tShirtMaterial;
    [SerializeField] private GameObject shieldBubble;
    [SerializeField] private GameObject magnet;

    #region On Start Initialized variables
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private List<SkinnedMeshRenderer> renderersList = new List<SkinnedMeshRenderer>();
    #endregion

    private bool isHit;
    public float health = 3;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        for(int i = 0; i < transform.childCount; i++)
        {
            SkinnedMeshRenderer renderer = transform.GetChild(i).GetComponent<SkinnedMeshRenderer>();
            if(renderer != null)
                renderersList.Add(renderer);
        }
    }

    private void FixedUpdate()
    {
        if(!isHit && GameController.instance.gameStarted)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (this.gameObject.transform.position.x > LevelBoundary.LEFT_SIDE)
                {
                    rb.AddForce(Vector3.left * FORCE * Time.deltaTime);
                    transform.rotation = Quaternion.Lerp(transform.rotation, leftRot, ROTATION_SPEED * Time.deltaTime);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (this.gameObject.transform.position.x < LevelBoundary.RIGHT_SIDE)
                {
                    rb.AddForce(Vector3.right * FORCE * Time.deltaTime);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rightRot, ROTATION_SPEED * Time.deltaTime);
                    //transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHit && GameController.instance.gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.Play(JUMP);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                animator.Play(SLIDE);
            }

            transform.Translate(Vector3.forward * 10 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, forwardRot, ROTATION_SPEED * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag.Equals(OBSTACLE))
        {
            if (shieldBubble.activeSelf)
            {
                SetShieldBubbleState(false);
                StartCoroutine(ResetCharacter(0, 2));
            }
            else
            {
                isHit = true;
                animator.Play(FALL_BACK);
                GameController.instance.decreaseHealthEvent.Invoke();
                if (health != 0)
                {
                    StartCoroutine(ResetCharacter(GetAnimClipTime(FALL_BACK), 2));
                }
                else
                {
                    Debug.LogError("GAME OVER");
                    PlayerPreferences.SetCurrencyAmount(GameController.instance.currencyCounter);
                    //TO DO: GAME OVER SCREEN 
                }
            }
        }
    }

    private IEnumerator ResetCharacter(float hitTime, float blinkTime)
    {
        yield return new WaitForSeconds(hitTime);
        isHit = false;
        capsuleCollider.isTrigger = transform;
        float startTime = Time.time;
        while (Time.time - startTime < blinkTime)
        {
            foreach (SkinnedMeshRenderer r in renderersList)
            {
                r.enabled = false;
            }
            yield return new WaitForSeconds(0.2f);
            foreach (SkinnedMeshRenderer r in renderersList)
            {
                r.enabled = true;
            }
            yield return new WaitForSeconds(0.2f);
        }
        capsuleCollider.isTrigger = false;
    }

    public float GetAnimClipTime(string name)
    {
        return animator.runtimeAnimatorController.animationClips.Where(n => n.name == name).FirstOrDefault().length;
    }

    public void SetGameState()
    {
        transform.position = new Vector3(0, 0, -4);
        transform.eulerAngles = Vector3.zero;
        animator.Play(RUN);
    }    

    public void SetStoreEnterState()
    {
        transform.position = new Vector3(-6.2f, 0.1f, 6.2f);
        transform.eulerAngles = new Vector3(0, 90, 0);
        capsuleCollider.isTrigger = true;
        animator.Play(CATWALK_FORWARD);
    }

    public void SetStoreExitState()
    {
        capsuleCollider.isTrigger = false;
        //animator.Play(CATWALK_BACKWARD);
    }

    public void SetTShirtMaterial(string path)
    {
        Debug.LogError(path);
        tShirtMaterial.mainTexture = Resources.Load<Texture>(path);
    }

    public void PlayGameOverAnim()
    {
        animator.Play("Tpose");
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        transform.localScale = new Vector3(.5f, .5f, 0.01f);
    }

    public void SetShieldBubbleState(bool state)
    {
        shieldBubble.SetActive(state);
    }

    public void SetMagnetState(bool state)
    {
        magnet.SetActive(state);
    }
}
