using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject slash;
    public GameObject altSlash;
    public GameObject upSlash;
    public GameObject downSlash;
    public ContactFilter2D enemyContactFilter2D;
    public enum AttackType
    {
        Slash,
        AltSlash,
        UpSlash,
        DownSlash
    };

    public void Play(AttackType attackType, ref List<Collider2D> colliders)
    {
        switch (attackType)
        {
            case AttackType.Slash:
                Physics2D.OverlapCollider(slash.GetComponent<Collider2D>(), enemyContactFilter2D, colliders);
                //音效
                slash.GetComponent<AudioSource>().Play();
                break;
            case AttackType.AltSlash:
                Physics2D.OverlapCollider(altSlash.GetComponent<Collider2D>(), enemyContactFilter2D, colliders);
                altSlash.GetComponent<AudioSource>().Play();
                //音效
                break;
            case AttackType.UpSlash:
                Physics2D.OverlapCollider(upSlash.GetComponent<Collider2D>(), enemyContactFilter2D, colliders);
                upSlash.GetComponent<AudioSource>().Play();
                //音效
                break;
            case AttackType.DownSlash:
                Physics2D.OverlapCollider(downSlash.GetComponent<Collider2D>(), enemyContactFilter2D, colliders);
                downSlash.GetComponent<AudioSource>().Play();
                //音效
                break;
            default:
                break;
        }
    }
}
