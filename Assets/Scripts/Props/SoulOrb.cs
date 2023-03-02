using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    private Health health;
    private Animator animator;

    private void Start()
    {
        health= FindObjectOfType<Health>();
        animator = GetComponent<Animator>();
    }

    public void DelayShowOrb(float delay)
    {
        StartCoroutine(ShowOrb(delay));
    }

    IEnumerator ShowOrb(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("Respawn");
    }

    public void HideOrb()
    {
        animator.SetTrigger("Hide");
    }

    public void ShowHealthItems()
    {
        StartCoroutine(health.ShowHealthItems());
    }

    public void HideHealthItems()
    {
        health.HideHealthItems();
    }
}
