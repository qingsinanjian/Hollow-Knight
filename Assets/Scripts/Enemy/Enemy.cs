using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Breakable
{
    int randomCount;
    public GameObject coin;

    [SerializeField]protected int minSpawnsCoins = 2;
    [SerializeField] protected int maxSpawnsCoins = 5;
    [SerializeField] protected int maxBumpXForce = 1;
    [SerializeField] protected int minBumpYForce = 3;
    [SerializeField] protected int maxBumpYForce = 5;

    protected bool isFacingRight;
    protected void Direction()
    {
        if (transform.localScale.x == 1)
        {
            isFacingRight = true;
        }
        else if (transform.localScale.x == -1)
        {
            isFacingRight = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetectCollision2D(collision);
    }

    protected virtual void DetectCollision2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("HeroDetector"))
        {
            FindObjectOfType<PlayerController>().TakeDamage();
            FindObjectOfType<HitPause>().Stop(0.5f, 0f);
        }
        if(isDead && collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    protected override void Dead()
    {
        base.Dead();
        SpawCoins();
    }

    public virtual void SpawCoins()
    {
        randomCount = Random.Range(minSpawnsCoins, maxSpawnsCoins);//2 3 4
        for (int i = 0; i < randomCount; i++)
        {
            GameObject geo = Instantiate(coin, transform.position, Quaternion.identity, transform.parent);
            Vector2 force = new Vector2(Random.Range(-maxBumpXForce, maxBumpXForce), Random.Range(minBumpYForce, maxBumpYForce));
            geo.transform.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
        }
    }
}
