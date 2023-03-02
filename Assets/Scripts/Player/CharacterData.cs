using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private bool isWeak;
    [SerializeField] private bool isDead;

    private Animator animator;
    private GameManager gameManager;
    private CharacterEffect characterEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        characterEffect = FindObjectOfType<CharacterEffect>();
    }

    private void Update()
    {
        CheckWeakHealth();
        CheckIsDead();
    }

    private void CheckWeakHealth()
    {
        if(health == 1 && !isWeak)
        {
            isWeak= true;
            characterEffect.DoEffect(CharacterEffect.EffectType.LowHealth, true);
        }
        else
        {
            isWeak = false;
            characterEffect.DoEffect(CharacterEffect.EffectType.LowHealth, false);
        }
    }

    private void CheckIsDead()
    {
        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        gameManager.SetEnableInput(false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("HeroDetector"), LayerMask.NameToLayer("EnemyDetector"), true);
        isDead = true;
        animator.SetTrigger("Dead");
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public void LoseHealth(int health)
    {
        this.health -= health; 
    }

    public bool GetDeadStatement()
    {
        CheckIsDead();
        return isDead;
    }
}
