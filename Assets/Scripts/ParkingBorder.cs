using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingBorder : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float maxAlpha = 1f;
    [SerializeField] private float transitionTime = 1f;
    private bool isPlayerStaying;

    public void PlayerEntered()
    {
        isPlayerStaying = true;
    }

    public void PlayerExited()
    {
        isPlayerStaying = false;
    }

    private void Update()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        float alpha = color.a;
        if (isPlayerStaying)
        {
            if (alpha < maxAlpha)
            {
                alpha += maxAlpha * Time.deltaTime / transitionTime;
                if (alpha > maxAlpha) alpha = maxAlpha;
            }
        }
        else
        {
            if (alpha > 0)
            {
                alpha -= maxAlpha * Time.deltaTime / transitionTime;
                if (alpha < 0) alpha = 0;
            }
        }
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
