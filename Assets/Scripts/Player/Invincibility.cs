using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Color normalColor;
    public Color flashColor;

    public int duration;

    public bool isInvincibility;

    public IEnumerator SetInvincibility()
    {
        isInvincibility= true;
        for (int i = 0; i < duration; i++)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color= flashColor;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color= normalColor;
        }
        isInvincibility= false;
    }
}
