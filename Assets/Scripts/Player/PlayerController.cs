using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动参数")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int moveChangeAni = 0;
    [SerializeField] private float jumpTimer = 0.4f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float hurtForce = 3f;
    Vector3 flippedScale = new Vector3(-1f, 1f, 1f);
    float moveX;
    float moveY;
    [Header("引用组件")]
    private CharacterEffect characterEffect;
    private Rigidbody2D rigidbody2D;
    private Animator animator;//Knight动画控制器
    private CinemaShaking cinemaShaking;
    private Attack attack;
    private GameManager gameManager;
    private AudioSource audioSource;
    private CharacterAudio characterAudio;
    [Header("状态判断")]
    private bool isOnGround;
    private bool isFacingRight;
    private bool canMove = true;
    bool firstLanding;
    [Header("攻击参数")]
    [SerializeField] float combTime = 0.4f;
    [SerializeField] private int slashDamage = 1;
    [SerializeField] private float downRecoilForce = 1f;
    [SerializeField] private float recoilForce = 4f;
    [SerializeField] private float slashIntervalTime = 0.5f;
    private float lastSlashTime;
    int slashCount = 0;

    void Start()
    {
        characterEffect = FindObjectOfType<CharacterEffect>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cinemaShaking = FindObjectOfType<CinemaShaking>();
        attack = FindObjectOfType<Attack>();
        canMove = true;
        gameManager = FindObjectOfType<GameManager>();
        audioSource= GetComponent<AudioSource>();
        characterAudio = FindObjectOfType<CharacterAudio>();
    }

    void Update()
    {
        ResetCombTime();
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    TakeDamage();
        //}
        Movement();
        Direction();
        Jump();
        PlayerAttack();
        animator.SetBool("FirstLanding", firstLanding);
    }

    private void Movement()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        if (canMove && gameManager.IsEnableInput())
        {
            rigidbody2D.velocity = new Vector2(moveX * moveSpeed, rigidbody2D.velocity.y);
        }
     
        if(moveX> 0)
        {
            moveChangeAni = 1;
        }else if(moveX < 0)
        {
            moveChangeAni = -1;
        }else moveChangeAni= 0;

        animator.SetInteger("movement", moveChangeAni);
        animator.SetFloat("VelocityY", rigidbody2D.velocity.y);
    }

    private void Direction()
    {
        if (gameManager.IsEnableInput())
        {
            if (moveX > 0)
            {
                isFacingRight = true;
                this.transform.localScale = flippedScale;
            }
            else if (moveX < 0)
            {
                isFacingRight = false;
                this.transform.localScale = Vector3.one;
            }
        }   
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            if (!gameManager.IsEnableInput()) return;
            if(isOnGround && jumpTimer <= 0) { jumpTimer = 0.01f; }
            jumpTimer -= Time.deltaTime;
            if(jumpTimer > 0)
            {
                rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
                animator.SetTrigger("jump");
                characterAudio.PlayAudio(CharacterAudio.AudioType.Jump, true);
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Grounding(collision, false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Grounding(collision, false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Grounding(collision, true);
    }

    private void Grounding(Collision2D col, bool exitState)
    {
        if(exitState)//离开为真
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                isOnGround = false;
            }
        }
        else
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !isOnGround && col.contacts[0].normal == Vector2.up)
            {
                characterEffect.DoEffect(CharacterEffect.EffectType.FallTrail, true);
                isOnGround = true;
                JumpCancel();
            }else if(col.gameObject.layer == LayerMask.NameToLayer("Terrain") && !isOnGround && col.contacts[0].normal == Vector2.down)
            {
                JumpCancel();
            }
        }
        animator.SetBool("isOnGround", isOnGround);
    }

    private void JumpCancel()
    {
        animator.ResetTrigger("jump");
        jumpTimer = 0.4f;
    }

    public void TakeDamage()
    {
        cinemaShaking.CinemaShake();
        StartCoroutine(FindObjectOfType<Invincibility>().SetInvincibility());
        FindObjectOfType<Health>().Hurt();
        if (isFacingRight)
        {
            rigidbody2D.velocity = new Vector2(1, 1) * hurtForce;
        }
        else
        {
            rigidbody2D.velocity = new Vector2(-1, 1) * hurtForce;
        }
        animator.Play("TakeDamage");
        characterAudio.PlayAudio(CharacterAudio.AudioType.TakeDamage, true);
    }

    private void PlayHitParticals()
    {
        characterEffect.DoEffect(CharacterEffect.EffectType.HitL, true);
        characterEffect.DoEffect(CharacterEffect.EffectType.HitR, true);

    }
    public void PlayerAttack()
    {
        if (Input.GetKeyDown(KeyCode.X))//且人物活着
        {
            if (!gameManager.IsEnableInput()) return;
            if (Time.time > lastSlashTime + slashIntervalTime)
            {              
                if (moveY > 0 && Time.time > lastSlashTime + combTime)
                {
                    SlashAndDetect(Attack.AttackType.UpSlash);
                    animator.Play("UpSlash");
                } else if (!isOnGround && moveY < 0 && Time.time > lastSlashTime + combTime)
                {
                    SlashAndDetect(Attack.AttackType.DownSlash);
                    animator.Play("DownSlash");
                }
                else
                {
                    slashCount++;
                    switch (slashCount)
                    {
                        case 1:
                            //Slash
                            SlashAndDetect(Attack.AttackType.Slash);
                            animator.Play("Slash");
                            break;
                        case 2:
                            //AltSlash
                            SlashAndDetect(Attack.AttackType.AltSlash);
                            animator.Play("AltSlash");
                            slashCount= 0;
                            break;
                        default:
                            break;
                    }
                }
                lastSlashTime = Time.time;
            }
        }
    }

    private void ResetCombTime()
    {
        if(Time.time > lastSlashTime + combTime && slashCount != 0)
        {
            slashCount = 0;
        }
    }

    private void SlashAndDetect(Attack.AttackType attackType)
    {
        List<Collider2D> colliders = new List<Collider2D>();
        attack.Play(attackType, ref colliders);
        bool hasEnemy = false;
        bool hasDamagePlayer= false;

        //检测是否是敌人
        foreach (Collider2D collider2D in colliders)
        {
            if(collider2D.gameObject.layer == LayerMask.NameToLayer("EnemyDetector"))
            {
                hasEnemy = true;
                break;
            }
        }

        foreach (Collider2D collider2D in colliders)
        {
            if (collider2D.gameObject.layer == LayerMask.NameToLayer("DamagePlayer"))
            {
                hasDamagePlayer = true;
                break;
            }
        }

        if (hasEnemy)
        {
            //Recoil
            if (attackType == Attack.AttackType.DownSlash)
            {
                AddDownRecoilForce();
            }
            else StartCoroutine(AddRecoilForce());
        }

        if (hasDamagePlayer)
        {
            if (attackType == Attack.AttackType.DownSlash)
            {
                AddDownRecoilForce();
            }
        }

        foreach (Collider2D col in colliders)
        {
            Breakable breakable = col.GetComponent<Breakable>();
            if(breakable != null)
            {
                breakable.Hurt(slashDamage, transform);
            }
        }

    }

    private void AddDownRecoilForce()
    {
        rigidbody2D.velocity.Set(rigidbody2D.velocity.x, 0);
        rigidbody2D.AddForce(Vector2.up * downRecoilForce, ForceMode2D.Impulse);
    }

    IEnumerator AddRecoilForce()
    {
        canMove = false;
        if (isFacingRight)
        {
            rigidbody2D.AddForce(Vector2.left * recoilForce, ForceMode2D.Impulse);
        }
        else
        {
            rigidbody2D.AddForce(Vector2.right * recoilForce, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.2f);
        canMove= true;
    }

    public void FirstLand()
    {
        StopInput();
        characterEffect.DoEffect(CharacterEffect.EffectType.BurstRocks, true);
    }
    public void StopInput()
    {
        gameManager.SetEnableInput(false);
        StopHorizontalMovement();
    }

    public void ResumeInput()
    {
        gameManager.SetEnableInput(true);
        firstLanding = true;
        FindObjectOfType<SoulOrb>().DelayShowOrb(0.1f);
    }

    private void StopHorizontalMovement()
    {
        Vector2 vec2 = rigidbody2D.velocity;
        vec2.x = 0;
        rigidbody2D.velocity = vec2;
        animator.SetInteger("movement", 0);
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
