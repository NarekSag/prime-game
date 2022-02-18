using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Const
    private const string JUMP = "Jump";
    private const string SLIDE = "Slide";
    private const string FALL_BACK = "Fall Back";
    private const string FALL_FORWARD = "Fall Forward";
    private const string OBSTACLE = "Obstacle";
    private const float FORCE = 100;
    private const float ROTATION_SPEED = 10;
    #endregion

    private Quaternion leftRot = Quaternion.AngleAxis(-45, Vector3.up);
    private Quaternion rightRot = Quaternion.AngleAxis(45, Vector3.up);
    private Quaternion forwardRot = Quaternion.AngleAxis(0, Vector3.up);

    #region On Start Initialized variables
    private Animator animator;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private List<SkinnedMeshRenderer> renderersList = new List<SkinnedMeshRenderer>();
    #endregion

    private bool isHit;
    private float health = 3;

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
        if(!isHit)
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
        if (!isHit)
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
            isHit = true;
            health--;
            animator.Play(FALL_BACK);
            if(health != 0)
            {
                StartCoroutine(ResetCharacter(GetAnimClipTime(FALL_BACK), 2));
            }
            else
            {
                Debug.LogError("GAME OVER");
                //TO DO: GAME OVER SCREEN 
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
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float length;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case FALL_BACK:
                    return length = clip.length;
                case FALL_FORWARD:
                    return length = clip.length;
            }
        }

        return 0;
    }
}
