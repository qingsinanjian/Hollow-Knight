using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoStone : Breakable
{
    [SerializeField] GameObject coin;
    [SerializeField] int minSpawnCoins;
    [SerializeField] int maxSpawnCoins;
    [SerializeField] float maxBumpXForce;
    [SerializeField] float minBumpYForce;
    [SerializeField] float maxBumpYForce;

    private Animator ani;
    private AudioSource audioSource;

    private void Start()
    {
        ani = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckIsDead();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Attack"))
        {
            Hurt(1, FindObjectOfType<Attack>().transform);
        }
    }

    public override void Hurt(int damage, Transform attackPosition)
    {
        base.Hurt(damage, attackPosition);
        Vector3 vector = attackPosition.position - this.transform.position;
        if(vector.x > 0)
        {
            //向左的特效
        }
        else
        {
            //向右的特效
        }
        SpawCoins();
        ani.SetTrigger("Hurt");
    }

    protected override void Dead()
    {
        base.Dead();
        //特效
        ani.SetTrigger("Dead");
    }

    private void SpawCoins()
    {
        int randomCount = Random.Range(minSpawnCoins, maxSpawnCoins);
        for (int i = 0; i < randomCount; i++)
        {
            GameObject geo = Instantiate(coin, transform.position, Quaternion.identity, transform);
            Vector2 vec = new Vector2(Random.Range(-maxBumpXForce, maxBumpXForce), Random.Range(minBumpYForce, maxBumpYForce));
            geo.GetComponent<Rigidbody2D>().AddForce(vec, ForceMode2D.Impulse);
        }
    }
}


